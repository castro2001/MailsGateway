

namespace Domain.Entidades.Mail
{
    public class Email
    {
        public string Uid { get; set; } // Identificador único del correo, puede ser un ID de base de datos o un UID de MailKit
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
    }
}
