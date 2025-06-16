using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Application.DTO
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre  es obligatorio.")]
        public string Nombre { get; set; } = string.Empty!;
        
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty!;
        
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        public string CorreoElectronico { get; set; } = string.Empty!;
        
        [Required(ErrorMessage = "La contraseña es obligatorio.")]
        public string Clave { get; set; } = string.Empty!;
        
        [Required(ErrorMessage = "Debe Repetir la contraseña.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string RepetirClave { get; set; } = string.Empty!;

        [Required(ErrorMessage = "La llave secreta es obligatoria para proseguir")]
        public string LlaveSecreta { get; set; } = string.Empty!;

        //[Required(ErrorMessage = "Solo se permiten imágenes JPG, JPEG o PNG..")]
        public IFormFile? Imagen { get; set; } 
        
        public string Perfil { get; set; } = string.Empty!;

       
    }
}
