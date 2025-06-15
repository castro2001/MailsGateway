

using Domain.Entidades.Seguridad;

namespace Domain.Entidades.Mail
{
    public class InboxMessage
    {
        public int Id { get; set; } // ID del mensaje en la base de datos
        public string Uid { get; set; } = string.Empty!;
        public string De { get; set; } = string.Empty!;
        public string Para { get; set; } = string.Empty!; // Correo del destinatario, por defecto vacío
        public string Asunto { get; set; } = string.Empty!;

        public string Contenido { get; set; } = string.Empty!;
        public DateTime Fecha { get; set; } = DateTime.Now; // Fecha por defecto al momento de crear el objeto
        public string InReplyTo { get; set; } = string.Empty!;
         
        public string EnviadoPor { get; set; } = string.Empty!;

        // Dominio con firma DKIM (puede usarse como "firmado por")
        public string FirmadoPor { get; set; } = string.Empty!;
        // Nivel de seguridad (TLS, etc.)
        public string Seguridad { get; set; } = string.Empty!;
        public string EncryptedUid { get; set; } = string.Empty!; // UID cifrado para mayor seguridad

        //Relación con el usuario Logueado
        public int? DestinatarioID { get; set; } // ID del usuario que recibió el correo
        public Usuario? Destinatario { get; set; } // Relación con la entidad Usuario
    }
}
