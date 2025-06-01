using Application.Interfaces;
using Domain.Entidades.Mail;
using MailKit;
using MailKit.Search;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Services.Mail
{
    public class EmailReaderMessageService : IEmailReaderMessageService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;
        public EmailReaderMessageService(IConfiguration configuration, IEmailConnectionProvider emailConnectionProvider)
        {
            _configuration = configuration;
            _emailConnectionProvider = emailConnectionProvider;
        }

        public Email DetalleMensajesEnviados(uint id)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var messageSentFolder = client.GetFolder(SpecialFolder.Sent);
            messageSentFolder.Open(FolderAccess.ReadOnly);


            // Buscar el mensaje por UID
            var uid = new UniqueId(id);
            var mensaje = messageSentFolder.GetMessage(uid);

            var detalle = new Email
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

        public List<Email> LeerMensajesEnviados()
        {
            var mensajes = new List<Email>();
            using var client = _emailConnectionProvider.GetImapClient();
            var messageSentFolder = client.GetFolder(SpecialFolder.Sent);
            messageSentFolder.Open(FolderAccess.ReadOnly);

            // Establecer el filtro de hora (hoy a las 18:57)
            DateTime horaInicio = DateTime.Today.AddHours(18).AddMinutes(57);

            // Buscar los correos entregados después de esa hora
            var uids = messageSentFolder.Search(SearchQuery.DeliveredAfter(horaInicio));

            foreach (var uid in uids)
            {
                var mensaje = messageSentFolder.GetMessage(uid);
                uint id = uid.Id;
                mensajes.Add(new Email
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
    }
}
