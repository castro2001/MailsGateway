

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
    }
}
