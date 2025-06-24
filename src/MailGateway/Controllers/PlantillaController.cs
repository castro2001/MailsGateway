using Application.DTO;
using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MailGateway.Controllers
{
    public class PlantillaController: Controller
    {
        private readonly INotificationStore _notificationStore;
        private readonly IEmailSenderService _emailSenderService;


        public PlantillaController(INotificationStore notificationStore, IEmailSenderService emailSenderService)
        {
            _notificationStore = notificationStore;
            _emailSenderService = emailSenderService;
        }

        [HttpGet]
        public IActionResult Seleccionar()
        {

            var plantillas = new List<PlantillaDTO>
            {
                new() { Id = 1, Nombre = "Confirmacion", ImagenUrl = "/Imagen/Confirmacion.png" },
                new() { Id = 2, Nombre = "Promocion", ImagenUrl = "/Imagen/Promocion.png" },
                new() { Id = 3, Nombre = "Notificacion", ImagenUrl = "/Imagen/Notificacion.png" },
                new() { Id = 4, Nombre = "Reporte", ImagenUrl = "/Imagen/Reporte.png" }
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
            _notificationStore.Agregar(modelo);

            return View(modelo);
        }

        [HttpGet]
        public IActionResult Editar(PlantillaCorreoDTO plantillaDTO)
        {
            var plantilla = _notificationStore.Obtener<PlantillaCorreoDTO>(x => x as PlantillaCorreoDTO)
                .FirstOrDefault(x => x.Id == plantillaDTO.Id);
            if (plantilla == null)
            {
                TempData["Error"] = "No se encontró la plantilla en memoria.";
                return RedirectToAction("Seleccionar");
            }
            return View(plantilla);
        }


        [HttpPost]
        public IActionResult Redactar(PlantillaCorreoDTO plantillaDTO)
        {
            if (string.IsNullOrWhiteSpace(plantillaDTO.Para))
            {
                ViewBag.Error = "El campo Para es obligatorio";
                return View();
            }

            if (string.IsNullOrWhiteSpace(plantillaDTO.Asunto))
            {
                ViewBag.Error = "El campo Asunto es obligatorio";
                return View();
            }
            var email = new EmailDTO
            {
                Para = plantillaDTO.Para,
                Asunto = plantillaDTO.Asunto,
                Contenido = plantillaDTO.ContenidoHtml,
            };

            var resultado = _emailSenderService.SendEmail(email);

            if (resultado.Success)
            {
                ViewBag.Success = "Correo enviado correctamente.";
                return View("Editar",new PlantillaCorreoDTO());
            }
            else
            {
                ViewBag.Error = $"Error al enviar correo: {resultado.ErrorMessage}";
                return View();
            }
        }



    }
}
