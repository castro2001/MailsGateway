using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Helper
{
    public class SecurityHelper
    {
        public static bool EsClaveSegura(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave) || clave.Length < 8)
                return false;

            bool tieneMayuscula = clave.Any(char.IsUpper);
            bool tieneMinuscula = clave.Any(char.IsLower);
            bool tieneNumero = clave.Any(char.IsDigit);
            bool tieneSimbolo = clave.Any(ch => !char.IsLetterOrDigit(ch));

            return tieneMayuscula && tieneMinuscula && tieneNumero && tieneSimbolo;
        }
        //HASH PASSWORD WITH SALT 1
        public static string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static bool EsCorreoGmailValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            // Verifica que sea formato email
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            return Regex.IsMatch(correo.Trim(), pattern, RegexOptions.IgnoreCase);
        }

    }
}
