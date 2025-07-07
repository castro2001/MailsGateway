using Microsoft.AspNetCore.Http;


namespace Application.DTO.Mail
{
    public class ComposeEmailDto: EmailMessageBase
    {
        // Para manejar imágenes y archivos adjuntos solo par enviar formuarios de correo
        public List<IFormFile> Imagenes { get; set; } = new List<IFormFile>();
        public List<IFormFile> ArchivosAdjuntos { get; set; } = new List<IFormFile>();

    }
}
