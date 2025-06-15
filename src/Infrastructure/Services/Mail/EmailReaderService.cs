using Application.Interfaces;
using Domain.Entidades.Mail;
using Infrastructure.Persistence.Context;
using MailKit;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Services.Mail
{
    public class EmailReaderService : IEmailReaderService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;
        private readonly INotificationStore _notificationStore;
        private readonly ApplicationDbContext _context;
        public EmailReaderService(
            IConfiguration configuration,
            INotificationStore notificationStore,
            IEmailConnectionProvider emailConnectionProvider,
            ApplicationDbContext context)
        {
            _configuration = configuration;
            _emailConnectionProvider = emailConnectionProvider;
            _notificationStore = notificationStore;
            _context = context;
        }

        public async Task<List<InboxMessage>> LeerMensajesRecibidos()
        {
            var mensajes = new List<InboxMessage>();
            using var client = _emailConnectionProvider.GetImapClient();
            // Seleccionar la bandeja de entrada
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Establecer el filtro de hora (hoy a las 18:57)
            DateTime horaInicio = DateTime.Today.AddHours(10).AddMinutes(57);

            // Buscar los correos entregados después de esa hora
            var uids = inbox.Search(SearchQuery.DeliveredAfter(horaInicio));

            //using var client = _emailConnectionProvider.GetImapClient();
            //var inbox = client.Inbox;
            //inbox.Open(FolderAccess.ReadOnly);

            //var desde = DateTime.UtcNow.AddDays(-1); // o configúralo como necesites
            //var uids = inbox.Search(SearchQuery.DeliveredAfter(desde));

            foreach (var uid in uids)
            {
                //if (!_context.InboxMessage.Any(e => e.Uid == uid.Id.ToString()))
                //{
                    var mensaje = await inbox.GetMessageAsync(uid);

                    var nuevo = new InboxMessage
                    {
                        DestinatarioID = 12, // reemplázalo dinámicamente si usas claims
                        Uid = uid.Id.ToString(),
                        De = mensaje.From.ToString(),
                        Para = mensaje.To.ToString(),
                        Asunto = mensaje.Subject,
                        Contenido = mensaje.HtmlBody ?? mensaje.TextBody ?? "(sin contenido)",
                        Fecha = mensaje.Date.DateTime,
                        InReplyTo = mensaje.InReplyTo ?? string.Empty,
                        EnviadoPor = mensaje.Headers["X-Sender"] ?? string.Empty,
                        FirmadoPor = mensaje.Headers["DKIM-Signature"]?.Split("d=")[1]?.Split(';')[0]?.Trim() ?? string.Empty,
                        Seguridad = mensaje.Headers["Received"]?.Contains("TLS") == true ? "TLS" : "Sin cifrado",
                    };

                    _context.InboxMessage.Add(nuevo);

                    // Notificación si es respuesta a otro mensaje
                    var originalMessage = _context.SentMessage.FirstOrDefault(s => s.MessageId == mensaje.InReplyTo);
                    if (originalMessage != null)
                    {
                        _context.Notification.Add(new Notification
                        {
                            Titulo = "Nuevo Mensaje Respondido",
                            Mensaje = $"{mensaje.From} ha respondido tu mensaje {originalMessage.Asunto}.",
                            Fecha = DateTime.UtcNow,
                            UsuarioId = originalMessage.RemitenteId,
                            Url = "/Home/Index"
                        });
                    }

                    mensajes.Add(nuevo); // agregamos a la lista a retornar
                //}
            }

            await _context.SaveChangesAsync();
            client.Disconnect(true);

            return mensajes.OrderByDescending(m => m.Fecha).ToList();
        }

        public InboxMessage DetalleCorreo(uint id)
        {
            using var client = _emailConnectionProvider.GetImapClient();
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            // Buscar el mensaje por UID
            var uid = new UniqueId(id);
            var mensaje = inbox.GetMessage(uid);

            // Encabezados completos
            var headers = mensaje.Headers;

            // Extraer valores específicos
            string enviadoPor = headers["X-Sender"] ?? headers["Return-Path"] ?? "";
            string firmadoPor = headers["DKIM-Signature"]?.Split("d=")[1]?.Split(';')[0]?.Trim() ?? "";
            string seguridad = headers["Received"]?.Contains("TLS") == true ? "Cifrado estándar (TLS)" : "Sin cifrado";
          
       

            var detalle = new InboxMessage
            {
                Para = mensaje.To.ToString(),
                De = mensaje.From.ToString(),
                Asunto = mensaje.Subject,
                Contenido = mensaje.HtmlBody ?? mensaje.TextBody,
                Fecha = mensaje.Date.DateTime,
                InReplyTo = mensaje.InReplyTo,
                EnviadoPor = enviadoPor,
                FirmadoPor = firmadoPor,
                Seguridad = seguridad
            };

            client.Disconnect(true);
            // Ordenar del más reciente al más antiguo
            
             
            return detalle;
        }

    
    }
}
