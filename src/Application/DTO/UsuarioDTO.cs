using Microsoft.AspNetCore.Http;

namespace Application.DTO
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty!;
        public string Apellido { get; set; } = string.Empty!;
        public string CorreoElectronico { get; set; } = string.Empty!;

        public string Clave { get; set; } = string.Empty!;
        public string RepetirClave { get; set; } = string.Empty!;
        public IFormFile? Imagen { get; set; } 
        public string Perfil { get; set; } = string.Empty!;
        public string LlaveSecreta { get; set; } = string.Empty!;

       
    }
}
