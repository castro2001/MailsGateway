
namespace Shared.Helper
{
    public class FechaHelper 
    {
        public static string FormatearFecha(DateTime fecha)
        {
            var ahora = DateTime.Now;
            var diferencia = ahora - fecha;
            string resultado;
            if (diferencia.TotalMinutes < 1)
                resultado = "hace unos segundos";
            else if (diferencia.TotalMinutes < 60)
                resultado = $"hace {(int)diferencia.TotalMinutes} minutos";
            else if (diferencia.TotalHours < 24)
                resultado = $"hace {(int)diferencia.TotalHours} hora{((int)diferencia.TotalHours > 1 ? "s" : "")}";
            else
                resultado = fecha.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("es-ES"));

            return $"{fecha:HH:mm} ({resultado})";
        }


        public static string FormatearHora(DateTime fecha)
        {
            var ahora = DateTime.Now;
            var diferencia = ahora - fecha;
            string resultado;
            if (diferencia.TotalMinutes < 1)
                resultado = "hace unos segundos";
            else if (diferencia.TotalMinutes < 60)
                resultado = $"hace {(int)diferencia.TotalMinutes} minutos";
            else if (diferencia.TotalHours < 24)
                resultado = $"hace {(int)diferencia.TotalHours} hora{((int)diferencia.TotalHours > 1 ? "s" : "")}";
            else
                resultado = fecha.ToString("dd MMM yyyy", new System.Globalization.CultureInfo("es-ES"));

            return $"{fecha:HH:mm} ";
        }

        public static string FormatearComoEmail(DateTime fecha)
        {
            // Cultura española para meses en minúscula y formato correcto
            var cultura = new System.Globalization.CultureInfo("es-ES");

            // "d MMM yyyy, H:mm" → 1 jun 2025, 8:07
            return fecha.ToString("d MMM yyyy, H:mm", cultura);
        }

        public static string FormatearDesdeTexto(DateTime fechaTexto)
        {
            var cultura = new System.Globalization.CultureInfo("es-ES");


            string diaSemana = cultura.DateTimeFormat.GetAbbreviatedDayName(fechaTexto.DayOfWeek).ToLower();
            return $"El {diaSemana}, {fechaTexto.ToString("dd MMM yyyy", cultura)} a las {fechaTexto:HH:mm}";
        }


    }
}
