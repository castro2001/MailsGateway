
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Application.Validations
{
    public class ClaveSeguraAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string clave)
                return new ValidationResult("La contraseña es obligatoria.");

            var esValida = Regex.IsMatch(clave, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");

            return esValida
                ? ValidationResult.Success
                : new ValidationResult("La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y símbolos.");
        }
    }
   
}
