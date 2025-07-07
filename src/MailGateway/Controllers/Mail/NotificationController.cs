using Application.DTO;
using Application.Interfaces;
using Domain.Entidades.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MailGateway.Controllers.Mail
{
    //[Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationStore _notificationStore;

        public NotificationController(INotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        [HttpGet]
        public IActionResult ObtenerMensajesEnviados()
        {
            var notis = _notificationStore.Obtener(n => n is NotificationDTO noti ? noti : null)
                .Where(n => n != null && !string.IsNullOrWhiteSpace(n.Mensaje) && n.Mensaje != "Sin mensaje")
                .ToList();

            if (!notis.Any())
            {
                return Json(new { status = "empty", mensaje = "No hay mensajes", data = new List<NotificationDTO>() });
            }

            return Json(new { status = "ok", mensaje = "Mensajes cargados", data = notis });
        }

        [HttpGet]
        public IActionResult ObtenerMensajesRecibidos()
        {
            var mensajesRecibidos = _notificationStore.Obtener(n => n is SentMessage sm ? sm : null)
                .Where(m => m != null)
                .ToList();

            if (!mensajesRecibidos.Any())
            {
                return Json(new { status = "empty", mensaje = "0", data = new List<SentMessage>() });
            }

            return Json(new { status = "ok", mensaje = "Mensajes cargados", data = mensajesRecibidos });
        }

  

        public IActionResult LimpiarNotificaciones()
        {
            _notificationStore.Limpiar();
            return Ok();
        }
    }
}
