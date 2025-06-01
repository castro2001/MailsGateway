using MailGateway.Models;
using MailKit;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using NuGet.Protocol;

namespace MailGateway.Services
{
    public class EmailReaderService :IEmailReaderService
    {
        private readonly IConfiguration _configuration;
       // private readonly TuDbContext _context;

        public EmailReaderService(IConfiguration configuration)
        {
            _configuration = configuration;
            //_context = context;
        }

        public List<EmailDTO> LeerMensajesRecibidos()
        {
            var mensajes = new List<EmailDTO>();
            using var client = new ImapClient();
            client.Connect(
                _configuration.GetSection("Email:Imap").Value,
                    Convert.ToInt32(_configuration.GetSection("Email:ImanPort").Value),
                 MailKit.Security.SecureSocketOptions.SslOnConnect
                    );
            client.Authenticate(
                    _configuration.GetSection("Email:Username").Value,
                    _configuration.GetSection("Email:Password").Value
                );
            // Seleccionar la bandeja de entrada
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Establecer el filtro de hora (hoy a las 18:57)
            DateTime horaInicio = DateTime.Today.AddHours(18).AddMinutes(57);

            // Buscar los correos entregados después de esa hora
            var uids = inbox.Search(SearchQuery.DeliveredAfter(horaInicio));

         

            foreach (var uid in uids)
            {
                var mensaje = inbox.GetMessage(uid);
                uint id = uid.Id;
                mensajes.Add(new EmailDTO
                {
                    Uid =id.ToString() ,
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

        public EmailDTO DetalleCorreo(uint id)
        {
            var client = new ImapClient();
            client.Connect(
          _configuration.GetSection("Email:Imap").Value,
              Convert.ToInt32(_configuration.GetSection("Email:ImanPort").Value),
           MailKit.Security.SecureSocketOptions.SslOnConnect
              );
            client.Authenticate(
                    _configuration.GetSection("Email:Username").Value,
                    _configuration.GetSection("Email:Password").Value
                );
     
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                // Buscar el mensaje por UID
                var uid = new UniqueId(id);
                var mensaje = inbox.GetMessage(uid);

                var detalle = new EmailDTO
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
