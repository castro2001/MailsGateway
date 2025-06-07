using Microsoft.AspNetCore.Http; // Necesaria para IFormFile

namespace Application.DTOS
{
    public class EmailDTO
    {
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public List<IFormFile> Imagenes { get; set; } // ← necesario
        public List<IFormFile> ArchivosAdjuntos { get; set; } // Adjuntos generales

    }

}
