using Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MailGateway.Controllers
{
    public class PlantillaController: Controller
    {
        [HttpGet]
        public IActionResult Seleccionar()
        {

            var plantillas = new List<PlantillaDTO>
            {
                new() { Id = 1, Nombre = "Confirmacion", ImagenUrl = "/img/confirmacion.png" },
                new() { Id = 2, Nombre = "Promocion", ImagenUrl = "/img/promocion.png" },
                new() { Id = 3, Nombre = "Notificacion", ImagenUrl = "/img/notificacion.png" },
                new() { Id = 3, Nombre = "Reporte", ImagenUrl = "/img/reporte.png" }
            };
             return View(plantillas);
        }

        [HttpGet]
        public IActionResult Visualizar(PlantillaDTO plantilla)
        {

            var ruta = Path.Combine("wwwroot", "Plantillas", $"{plantilla.Nombre}.html");

            if (!System.IO.File.Exists(ruta))
            {
                TempData["Error"] = "No se encontró la plantilla seleccionada.";
                return RedirectToAction("Seleccionar");
            }
            string html = System.IO.File.ReadAllText(ruta);

            var modelo = new PlantillaCorreoDTO
            {
                Id = plantilla.Id,
                NombrePlantilla = plantilla.Nombre,
                ContenidoHtml = html
            };

            return View(modelo);
        }

        [HttpGet]
        public IActionResult Editar(PlantillaCorreoDTO plantillaDTO)
        {
            var plantilla = new PlantillaCorreoDTO()
            {
                Id = plantillaDTO.Id,
                NombrePlantilla = plantillaDTO.NombrePlantilla,
                ContenidoHtml = plantillaDTO.ContenidoHtml
            };

            return View(plantilla);
        }


        }
    }
