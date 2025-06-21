using Application.Interfaces;
using Domain.Entidades;
using Domain.Entidades.Mail;
using Infrastructure.Persistence.Context;
using MailKit;
using MailKit.Search;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    //public class EmailBackgroundService : BackgroundService
    //{
    //    private readonly IServiceProvider _serviceProvider;
    //    private readonly ILogger<EmailBackgroundService> _logger;

    //    public EmailBackgroundService(IServiceProvider serviceProvider, ILogger<EmailBackgroundService> logger)
    //    {
    //        _serviceProvider = serviceProvider;
    //        _logger = logger;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        _logger.LogInformation("📧 Servicio de correo iniciado a las {time}", DateTime.UtcNow);

    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            _logger.LogInformation("🔄 Iniciando ciclo de revisión de correos a las {time}", DateTime.UtcNow);

    //            using var scope = _serviceProvider.CreateScope();
    //            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //            var emailConnectionProvider = scope.ServiceProvider.GetRequiredService<IEmailConnectionProvider>();

    //            try
    //            {
    //                using var client = emailConnectionProvider.GetImapClient();
    //                var inbox = client.Inbox;
    //                inbox.Open(FolderAccess.ReadOnly);

    //                var desde = DateTime.UtcNow.AddDays(-1);
    //                var uids = inbox.Search(SearchQuery.DeliveredAfter(desde));

    //                _logger.LogInformation("🔍 Se encontraron {cantidad} correos nuevos desde {desde}", uids.Count, desde);

    //                foreach (var uid in uids)
    //                {
    //                    if (!context.InboxMessage.Any(e => e.Uid == uid.Id.ToString()))
    //                    {
    //                        var mensaje = await inbox.GetMessageAsync(uid, stoppingToken);

    //                        _logger.LogInformation("📥 Nuevo mensaje de {de} - Asunto: {asunto}", mensaje.From, mensaje.Subject);

    //                        var nuevo = new InboxMessage
    //                        {
    //                            Uid = uid.Id.ToString(),
    //                            De = mensaje.From.ToString(),
    //                            Para = mensaje.To.ToString(),
    //                            Asunto = mensaje.Subject,
    //                            Contenido = mensaje.HtmlBody ?? mensaje.TextBody ?? "(sin contenido)",
    //                            Fecha = mensaje.Date.DateTime,
    //                            InReplyTo = mensaje.InReplyTo ?? string.Empty,
    //                            EnviadoPor = mensaje.Headers["X-Sender"] ?? string.Empty,
    //                            FirmadoPor = mensaje.Headers["DKIM-Signature"]?.Split("d=")[1]?.Split(';')[0]?.Trim() ?? string.Empty,
    //                            Seguridad = mensaje.Headers["Received"]?.Contains("TLS") == true ? "TLS" : "Sin cifrado",
    //                        };

    //                        context.InboxMessage.Add(nuevo);

    //                        var originalMessage = context.SentMessage.FirstOrDefault(s => s.MessageId == mensaje.InReplyTo);
    //                        if (originalMessage != null)
    //                        {
    //                            context.Notification.Add(new Notification
    //                            {
    //                                Titulo = "Nuevo Mensaje Respondido",
    //                                Mensaje = $"{mensaje.From} ha respondido tu mensaje {originalMessage.Asunto}.",
    //                                Fecha = DateTime.UtcNow,
    //                                UsuarioId = originalMessage.RemitenteId,
    //                                Url = "/Home/Index"
    //                            });
    //                        }
    //                    }
    //                }

    //                await context.SaveChangesAsync(stoppingToken);
    //                _logger.LogInformation("✅ Mensajes guardados correctamente en la base de datos.");
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogError(ex, "❌ Error al procesar los correos.");
    //            }

    //            _logger.LogInformation("⏳ Esperando 2 minutos para el próximo ciclo...");
    //            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
    //        }

    //        _logger.LogInformation("🛑 Servicio de correo detenido.");
    //    }
    //}
}
