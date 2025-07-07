using Domain.Entidades.Mail;
using Microsoft.AspNetCore.Http; // Necesaria para IFormFile

namespace Application.DTO
{
    public class EmailDTO
    {
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string ContenidoRespuesta { get; set; }
        // Para manejar imágenes y archivos adjuntos solo par enviar formuarios de correo
        public List<IFormFile> Imagenes { get; set; }
        public List<IFormFile> ArchivosAdjuntos { get; set; }

      
    }


}
