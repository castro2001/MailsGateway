using Application.Interfaces;
using Domain.Entidades.Mail;
using MailKit;
using MailKit.Search;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Services.Mail
{
    public class EmailReaderService : IEmailReaderService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;
        private readonly INotificationStore _notificationStore;
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

        public List<InboxMessage> LeerMensajesRecibidos()
        {
            var mensajes = new List<InboxMessage>();
            using var client = _emailConnectionProvider.GetImapClient();
            // Seleccionar la bandeja de entrada
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Establecer el filtro de hora (hoy a las 18:57)
            //DateTime horaInicio = DateTime.Today.AddHours(18).AddMinutes(57);
            DateTime horaInicio = DateTime.UtcNow.AddDays(-1);
            // Buscar los correos entregados después de esa hora
            var uids = inbox.Search(SearchQuery.DeliveredAfter(horaInicio));
            foreach (var uid in uids)
            {
                var mensaje = inbox.GetMessage(uid);
                uint id = uid.Id;

                // Encabezados completos
                var headers = mensaje.Headers;
                // Extraer valores específicos
                string enviadoPor = headers["X-Sender"] ?? headers["Return-Path"] ?? "";
                string firmadoPor = headers["DKIM-Signature"]?.Split("d=")[1]?.Split(';')[0]?.Trim() ?? "";
                string seguridad = headers["Received"]?.Contains("TLS") == true ? "Cifrado estándar (TLS)" : "Sin cifrado";
                // Obtener todos los encabezados
                var todosLosEncabezados = mensaje.Headers;
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
                    Fecha = mensaje.Date.DateTime

                });
            }
            client.Disconnect(true);


            return mensajes;
        }

        public InboxMessage DetalleCorreo(uint id)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Buscar el mensaje por UID
            var uid = new UniqueId(id);
            var mensaje = inbox.GetMessage(uid);

            var detalle = new InboxMessage
            {
                Para = mensaje.To.ToString(),
                De = mensaje.From.ToString(),
                Asunto = mensaje.Subject,
                Contenido = mensaje.HtmlBody ?? mensaje.TextBody,
                Fecha = mensaje.Date.DateTime
            };

            client.Disconnect(true);

            return detalle;
        }


    }
}
