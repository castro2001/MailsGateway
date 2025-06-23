using System;

namespace Application.DTO
{
    public class PlantillaDTO
    {
        
        public int Id { get; set; }
        public string Nombre { get; set; } = "Nombre de la Plantilla";
        public string ImagenUrl { get; set; } = "Url de la Imagen de la plantilla";
        public string NombreArchivoHtml { get; set; } = "Archivo de la plantilla"; // ejemplo: "plantilla1.html"
    }
}
