using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty!;
        public string Apellido { get; set; } = string.Empty!;
        public string CorreoElectronico { get; set; } = string.Empty!;

        public string Clave = string.Empty!;
        public string RepetirClave = string.Empty!;
        public string Perfil { get; set; } = string.Empty!;
    }
}
