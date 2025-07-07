
namespace Domain.Entidades.Mail
{
    public class MensajeAdjunto
    {
        public string Nombre { get; set; }
        public string TipoMime { get; set; }
        public byte[] Datos { get; set; }
        public bool EsImagen { get; set; }
        public string ContentId { get; set; } // Para imágenes embebidas
    }

}
