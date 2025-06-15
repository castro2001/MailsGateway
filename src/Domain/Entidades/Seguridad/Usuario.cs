using Domain.Entidades.Mail;
using System;
using System.Collections.Generic;

namespace Domain.Entidades.Seguridad
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty!;
        public string Apellido { get; set; } = string.Empty!;
        public string CorreoElectronico { get; set; } = string.Empty!;

        public string Clave { get; set; } = string.Empty!;
        public string Perfil { get; set; } = string.Empty!;

        public DateTime FechaCreacion { get; set; } 
        public DateTime? FechaModificacion { get; set; }
        public string PasswordSecret { get; set; } = string.Empty!;

        //Relacion con las demas entidades
        //Los mensajes enviados por el usuario
        public ICollection<SentMessage> MensajesEnviados { get; set; } = new List<SentMessage>();

        // La Entidad Email recibe los mensajes bandeja de entrada y salida
        public ICollection<InboxMessage> EmailRecibidos { get; set; } = new List<InboxMessage>();
        // La Entidad Email recibe los mensajes bandeja de entrada y salida
        public ICollection<Notification> Notificaciones { get; set; } = new List<Notification>();

    }
}
