using System;
using System.Collections.Generic;

namespace Domain.Entidades.Seguridad
{
    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty!;
        public string apellido { get; set; } = string.Empty!;
        public string correoElectronico { get; set; } = string.Empty!;

        public string clave = string.Empty!;
        public string perfil { get; set; } = string.Empty!;

    }
}
