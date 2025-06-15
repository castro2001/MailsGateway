
namespace Domain.Entidades.Mail
{
    public class Attachment
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; } = string.Empty!;
        public byte[] Contenido { get; set; } = Array.Empty<byte>();
        public string TipoMime { get; set; } = string.Empty!;

        // Relación opcional con mensaje enviado
        public string? SentMessageId { get; set; }
        public SentMessage? SentMessage { get; set; }

        // Relación opcional con mensaje recibido
        public string? InboxMessageUid { get; set; }
        public InboxMessage? InboxMessage { get; set; }
    }

}
