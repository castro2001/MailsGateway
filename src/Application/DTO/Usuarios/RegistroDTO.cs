using Application.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Application.DTO.Usuarios
{
    public class RegistroDTO
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MinLength(5, ErrorMessage = "El nombre debe tener al menos 5 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MinLength(5, ErrorMessage = "El nombre debe tener al menos 5 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        // Aquí podrías usar una validación personalizada:
       // [ClaveSeguraAttribute]
        public string Clave { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe repetir la contraseña.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string RepetirClave { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar su contraseña de aplicación.")]
        [MaxLength(16, ErrorMessage = "La contraseña de aplicación no puede superar los 16 caracteres.")]
        public string LlaveSecreta { get; set; } = string.Empty;

        public IFormFile? Imagen { get; set; }

        public string? Perfil { get; set; }

    }
}
