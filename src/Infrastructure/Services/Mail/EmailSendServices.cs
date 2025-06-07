using Application.DTO;
using Application.DTOS;
using Application.Interfaces;
using Domain.Entidades.Mail;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
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

        public EmailResponse SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration["Email:Username"]));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;

            var rootPath = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(rootPath, "Views", "Shared", "Mail", "EmailTemplate.cshtml");

            string htmlTemplate = File.ReadAllText(templatePath);
            string bodyHtml = htmlTemplate.Replace("{{Contenido}}", request.Contenido);

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



    }
}
