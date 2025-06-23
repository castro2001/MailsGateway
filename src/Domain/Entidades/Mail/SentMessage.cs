using Domain.Entidades.Seguridad;

namespace Domain.Entidades.Mail
{
    public class SentMessage
    {
        public string Id { get; set; } 
        public string? MessageId { get; set; } // este ID lo usan los clientes de correo para "responder"
        public string Para { get; set; } = string.Empty!; // Correo del destinatario, por defecto vacío
        public string? Asunto { get; set; } 
        public DateTime FechaEnvio { get; set; }
        public string Uid { get; set; } = string.Empty!;
        public string? InReplyTo { get; set; } // el ID del mensaje al que se está respondiendo, si aplica

        //Relacionado con el usuario que envió el mensaje
        public int? RemitenteId { get; set; } // ID del usuario que envió el mensaje
        public Usuario? Remitente { get; set; } // Relación con la entidad Usuario
    }
}
