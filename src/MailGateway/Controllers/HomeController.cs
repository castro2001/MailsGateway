using MailGateway.Models;
using Application.Interfaces;
using Application.DTOS;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MailGateway.Helpers;
using Application.Interfaces.Helper;
namespace MailGateway.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailReaderService _emailReaderService;
       

        public HomeController(ILogger<HomeController> logger, IEmailReaderService emailReaderService
            )
        {
            _logger = logger;
            _emailReaderService = emailReaderService;
            
        }

        public IActionResult Index()
        {
            var correos = _emailReaderService.LeerMensajesRecibidos();
            return View(correos);
        }

        public IActionResult Detalle( string uid )
        {
            string uidDesencriptado = CryptoHelper.Decrypt(uid);
            uint uidValor = uint.Parse(uidDesencriptado);
            var correo = _emailReaderService.DetalleCorreo(uidValor);
            return View(correo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
