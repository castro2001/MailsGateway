using Application.DTO;
using Application.DTO.Mail;
using Application.Interfaces;
using Domain.Entidades.Mail;
using MailKit;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PreMailer.Net;

using System.Text.RegularExpressions;


namespace Infrastructure.Services.Mail
{
    public class EmailSendServices : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;
        private readonly INotificationStore _notificationStore;

        public EmailSendServices(IConfiguration configuration, IEmailConnectionProvider emailConnectionProvider, INotificationStore notificationStore)
        {
            _configuration = configuration;
            _emailConnectionProvider = emailConnectionProvider;
            _notificationStore = notificationStore;
        }

        public EmailResponse SendEmail(ComposeEmailDto request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;

            var rootPath = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(rootPath, "Views", "Shared", "Mail", "EmailTemplate.cshtml");

            string htmlTemplate = File.ReadAllText(templatePath);
            string bodyHtml = htmlTemplate.Replace("{{Contenido}}", request.Contenido);
            var preMailer = new PreMailer.Net.PreMailer(bodyHtml);
            var resultado = preMailer.MoveCssInline();
            bodyHtml = resultado.Html;


            var builder = new BodyBuilder
            {
                HtmlBody = bodyHtml,
                TextBody = StripHtml(request.Contenido)
            };

            // ✅ Buscar imágenes en base64
            var base64Images = Regex.Matches(bodyHtml, @"<img[^>]*src=['""]data:image/(?<type>[^;]+);base64,(?<data>[^'""]+)['""][^>]*>");

            foreach (Match match in base64Images)
            {
                string imageType = match.Groups["type"].Value;
                string base64Data = match.Groups["data"].Value;
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                string cid = Guid.NewGuid().ToString(); // nombre único

                var image = builder.LinkedResources.Add($"{cid}.{imageType}", imageBytes);
                image.ContentId = cid;
                image.ContentType.MediaType = $"image/{imageType}";
                image.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                // Reemplazar la imagen base64 en el HTML por cid
                string originalImgTag = match.Groups[0].Value;
                string newImgTag = $"<img src=\"cid:{cid}.{imageType}\" />";
                bodyHtml = bodyHtml.Replace(originalImgTag, newImgTag);
            }
            // Agregar archivos adjuntos que no son imágenes embebidas
            if (request.ArchivosAdjuntos != null && request.ArchivosAdjuntos.Any())
            {
                foreach (var archivo in request.ArchivosAdjuntos)
                {
                    using var ms = new MemoryStream();
                    archivo.CopyTo(ms);
                    var bytes = ms.ToArray();

                    builder.Attachments.Add(archivo.FileName, bytes, ContentType.Parse(archivo.ContentType));
                }
            }

            builder.HtmlBody = bodyHtml; // importante: asignar de nuevo el HTML procesado
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = _emailConnectionProvider.GetSmtpClient();
                smtp.Send(email);
                smtp.Disconnect(true);

                _notificationStore.Agregar(new NotificationDTO
                {
                    Titulo = "Nuevo mensaje enviado",
                    Mensaje = $"El correo a {request.Para} se ha enviado correctamente.",
                    Icono = "fas fa-envelope",
                    Url = "/Enviados/Index"
                });

                return new EmailResponse
                {
                    Success = true,
                    Asunto = request.Asunto,
                    MessageId = email.MessageId
                };
            }
            catch (Exception ex)
            {
                return new EmailResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        private string StripHtml(string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty);
        }
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }

        public EmailResponse ReenviarCorreo(ForwardEmailDto mensaje)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            // Buscar el mensaje por UID
            var uid = new UniqueId(uint.Parse(mensaje.Uid));
            var mensajeOriginal = inbox.GetMessage(uid);

            // Crear un nuevo mensaje para reenviar
            var mensajeReenviado = new MimeMessage();
            mensajeReenviado.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
            mensajeReenviado.To.Add(MailboxAddress.Parse(mensaje.NuevoDestinatario));
            mensajeReenviado.Subject = "Fwd: " + mensajeOriginal.Subject;

            var builder = new BodyBuilder();

            // Reenviar contenido del mensaje original
            var cuerpoOriginal = mensajeOriginal.HtmlBody ?? mensajeOriginal.TextBody ?? "";
            builder.HtmlBody = $"<p>---------- Mensaje reenviado ---------</p>" +
                               $"<p><strong>De:</strong> {mensajeOriginal.From}</p>" +
                               $"<p><strong>Asunto:</strong> {mensajeOriginal.Subject}</p>" +
                               $"<p><strong>Fecha:</strong> {mensajeOriginal.Date}</p>" +
                               $"<hr />{cuerpoOriginal}";


            // Reenviar adjuntos (opcional)
            foreach (var adjunto in mensajeOriginal.Attachments)
            {
                if (adjunto is MimePart part)
                {
                    string mimeType = part.ContentType.MimeType;
                    if (!mimeType.StartsWith("image/") && !mimeType.Equals("application/pdf"))
                        continue; // No reenviar archivos sospechosos

                    using var stream = new MemoryStream();
                    part.Content.DecodeTo(stream);
                    stream.Position = 0;
                    builder.Attachments.Add(part.FileName, stream.ToArray(), ContentType.Parse(part.ContentType.MimeType));
                }
            }
            mensajeReenviado.Body = builder.ToMessageBody();
            try
            {
                using var smtp = _emailConnectionProvider.GetSmtpClient();
                smtp.Send(mensajeReenviado);
                smtp.Disconnect(true);

                _notificationStore.Agregar(new NotificationDTO
                {
                    Titulo = "Mensaje del Correo Reenviado",
                    Mensaje = $"El correo se ha enviado correctamente a {mensaje.NuevoDestinatario}. ",
                    Icono = "fas fa-envelope",
                    Url = "/Enviados/Index"
                });

                return new EmailResponse
                {
                    Success = true,
                    Asunto = mensaje.Asunto
                };
            }
            catch (Exception ex)
            {
                return new EmailResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public EmailResponse ResponderCorreo(ReplyEmailDTO mensaje)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var uid = new UniqueId(uint.Parse(mensaje.Uid));
            var mensajeOriginal = inbox.GetMessage(uid);

            var mensajeRespuesta = new MimeMessage();
            mensajeRespuesta.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
            mensajeRespuesta.To.AddRange(mensajeOriginal.ReplyTo.Count > 0 ? mensajeOriginal.ReplyTo : mensajeOriginal.From);
            mensajeRespuesta.Subject = "Re: " + mensajeOriginal.Subject;

            var builder = new BodyBuilder();

            // Contenido del usuario (mensaje personalizado)
            string contenidoUsuario = mensaje.ContenidoRespuesta;

            // Cuerpo del mensaje original (encabezado tipo reply)
            var cuerpoOriginal = mensajeOriginal.HtmlBody ?? mensajeOriginal.TextBody ?? "";
            string replyFormat = $"<br><br>----- Mensaje original -----<br>" +
                                 $"<strong>De:</strong> {mensajeOriginal.From}<br>" +
                                 $"<strong>Fecha:</strong> {mensajeOriginal.Date}<br>" +
                                 $"<strong>Asunto:</strong> {mensajeOriginal.Subject}<br><br>" +
                                 $"{cuerpoOriginal}";

            builder.HtmlBody = contenidoUsuario + replyFormat;

            mensajeRespuesta.Body = builder.ToMessageBody();

            try
            {
                using var smtp = _emailConnectionProvider.GetSmtpClient();
                smtp.Send(mensajeRespuesta);
                smtp.Disconnect(true);

                _notificationStore.Agregar(new NotificationDTO
                {
                    Titulo = "Mensaje de respuesta enviado",
                    Mensaje = $"Se respondió correctamente a {mensajeRespuesta.To}.",
                    Icono = "fas fa-reply",
                    Url = "/Enviados/Index"
                });

                return new EmailResponse
                {
                    Success = true,
                    Asunto = mensajeRespuesta.Subject
                };
            }
            catch (Exception ex)
            {
                return new EmailResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

    
    }
}
