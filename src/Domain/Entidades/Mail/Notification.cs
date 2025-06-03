using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Mail
{
    public class Notification
    {
        public string Titulo { get; set; } = string.Empty!;
        public string Mensaje { get; set; } = string.Empty!;
        public string Icono { get; set; } = string.Empty!;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Url { get; set; } = string.Empty!;
    }
}
