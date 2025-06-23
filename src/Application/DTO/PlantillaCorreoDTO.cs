using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PlantillaCorreoDTO
    {
        public int Id { get; set; }
        public string NombrePlantilla { get; set; } = "";
        public string ContenidoHtml { get; set; } = "Contenido De la Plantilla";

        // Parámetros dinámicos (opcional)
        public string Titulo { get; set; } = "Titulo";
        public string Parrafo { get; set; } = "Parraffos";
        public string Enlace { get; set; } = "URL";


    }
}
