using Application.DTO;
using Application.Interfaces;
using Bogus;
using Domain.Entidades.Mail;
using MailKit;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailGateway.Hubs.Mail;

namespace Infrastructure.Services.Mail
{
    public class EmailReaderService : IEmailReaderService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;
        private readonly INotificationStore _notificationStore;
        private readonly IHasContext <MailHub>
        public EmailReaderService(
            IConfiguration configuration,
            INotificationStore notificationStore,
            IEmailConnectionProvider emailConnectionProvider
        )
        {
            _configuration = configuration;
            _emailConnectionProvider = emailConnectionProvider;
            _notificationStore = notificationStore;
           
        }

        public List<InboxMessage> LeerMensajesRecibidos(out string errorMessage)
        {
            var mensajes = new List<InboxMessage>();
            errorMessage = null;
            try
            {

            
            using var client = _emailConnectionProvider.GetImapClient();
           
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                DateTime horaInicio = DateTime.UtcNow.AddDays(-1);
                var uids = inbox.Search(SearchQuery.DeliveredAfter(horaInicio));

                foreach (var uid in uids)
                {
                    var mensaje = inbox.GetMessage(uid);
                    uint id = uid.Id;

                    var adjuntos = new List<MensajeAdjunto>(); // ✅ Declarar dentro del foreach

                    foreach (var adjunto in mensaje.Attachments)
                    {
                        if (adjunto is MimePart part)
                        {
                            var extension = Path.GetExtension(part.FileName)?.ToLowerInvariant() ?? "";
                            var tipoMime = part.ContentType.MimeType.ToLowerInvariant();

                            // ⚠️ Filtrar archivos peligrosos
                            if (EsAdjuntoPeligroso(extension, tipoMime))
                            {
                                Console.WriteLine($"[Bloqueado por seguridad] {part.FileName} - {tipoMime}");
                                continue;
                            }

                            using var stream = new MemoryStream();
                            part.Content.DecodeTo(stream);

                            adjuntos.Add(new MensajeAdjunto
                            {
                                Nombre = part.FileName,
                                TipoMime = part.ContentType.MimeType,
                                Datos = stream.ToArray(),
                                EsImagen = part.ContentType.MediaType == "image",
                                ContentId = part.ContentId
                            });
                        }
                    }

                    // Encabezados y registro
                    var headers = mensaje.Headers;
                    string enviadoPor = headers["X-Sender"] ?? headers["Return-Path"] ?? "";
                    string firmadoPor = headers["DKIM-Signature"]?.Split("d=")[1]?.Split(';')[0]?.Trim() ?? "";
                    string seguridad = headers["Received"]?.Contains("TLS") == true ? "Cifrado estándar (TLS)" : "Sin cifrado";

                    _notificationStore.Agregar(new SentMessage
                    {
                        Id = id.ToString(),
                        MessageId = mensaje.MessageId,
                        Para = mensaje.To.ToString(),
                        Asunto = mensaje.Subject,
                        FechaEnvio = mensaje.Date.DateTime
                    });

                    _notificationStore.VerificarYNotificarRespuesta(mensaje.InReplyTo, mensaje.From.ToString());

                    mensajes.Add(new InboxMessage
                    {
                        Uid = id.ToString(),
                        De = mensaje.From.ToString(),
                        Para = mensaje.To.ToString(),
                        Asunto = mensaje.Subject,
                        Contenido = mensaje.HtmlBody ?? mensaje.TextBody,
                        Fecha = mensaje.Date.DateTime,
                        Adjuntos = adjuntos
                    });
                }

                client.Disconnect(true);
                return mensajes;

            }
            catch (ApplicationException ex)
            {
                errorMessage = ex.Message;
                return mensajes;
            }

        }

    


        public InboxMessage DetalleCorreo(uint id)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Buscar el mensaje por UID
            var uid = new UniqueId(id);
            var mensaje = inbox.GetMessage(uid);
            var adjuntos = new List<MensajeAdjunto>(); // ✅ Declarar dentro del foreach

            foreach (var adjunto in mensaje.Attachments)
            {
                if (adjunto is MimePart part)
                {
                    var extension = Path.GetExtension(part.FileName)?.ToLowerInvariant() ?? "";
                    var tipoMime = part.ContentType.MimeType.ToLowerInvariant();

                    // ⚠️ Filtrar archivos peligrosos
                    if (EsAdjuntoPeligroso(extension, tipoMime))
                    {
                        Console.WriteLine($"[Bloqueado por seguridad] {part.FileName} - {tipoMime}");
                        continue;
                    }

                    using var stream = new MemoryStream();
                    part.Content.DecodeTo(stream);

                    adjuntos.Add(new MensajeAdjunto
                    {
                        Nombre = part.FileName,
                        TipoMime = part.ContentType.MimeType,
                        Datos = stream.ToArray(),
                        EsImagen = part.ContentType.MediaType == "image",
                        ContentId = part.ContentId
                    });
                }
            }


            var detalle = new InboxMessage
            {
                Para = mensaje.To.ToString(),
                De = mensaje.From.ToString(),
                Asunto = mensaje.Subject,
                Contenido = mensaje.HtmlBody ?? mensaje.TextBody,
                Fecha = mensaje.Date.DateTime,
                Adjuntos = adjuntos // ✅ Asegúrate de agregar esto
            };

            client.Disconnect(true);

            return detalle;
        }


        // ✅ Función para validar archivos peligrosos
        private bool EsAdjuntoPeligroso(string extension, string mimeType)
        {
            var extensionesProhibidas = new HashSet<string> {
                ".exe", ".bat", ".cmd", ".js", ".vbs", ".ps1", ".dll", ".com", ".scr", ".msi"
            };
            var mimeProhibidos = new HashSet<string> {
                "application/x-msdownload", "application/javascript", "application/x-msdos-program"
            };

            return extensionesProhibidas.Contains(extension) || mimeProhibidos.Contains(mimeType);
        }


    }
}
