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
        public string NombrePlantilla { get; set; } = string.Empty!;
        public string ContenidoHtml { get; set; } = string.Empty!;

        // Parámetros dinámicos (opcional)
        public string Para { get; set; } = string.Empty!;
        public string Asunto { get; set; } = string.Empty!;
        //public string Enlace { get; set; } = "URL";


    }
}
