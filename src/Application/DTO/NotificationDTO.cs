using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class NotificationDTO
    {
        public string Titulo { get; set; }="Titulo de la Notificación";
        public string Mensaje { get; set; }="Mensaje de la Notificación";
        public string Icono { get; set; } = "fas fa-envelope";
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Url { get; set; } = "Url del destino";
    }
}
