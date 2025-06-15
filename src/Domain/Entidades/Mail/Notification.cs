using Domain.Entidades.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Mail
{
    public class Notification
    {
        public int Id { get; set; }// clave primaria
        public string Titulo { get; set; } = string.Empty!;
        public string Mensaje { get; set; } = string.Empty!;
        public string Icono { get; set; } = string.Empty!;
        public string Tipo { get; set; } = string.Empty!; // Ej: 'Mensaje', 'Sistema', 'Alerta'

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Url { get; set; } = string.Empty!;

        public bool Leido { get; set; } = false;

        // Relación con el usuario
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
