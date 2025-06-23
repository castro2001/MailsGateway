using MailGateway.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
namespace MailGateway.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailReaderService _emailReaderService;
        private readonly CryptoHelper _cryptoHelper;


        public HomeController(ILogger<HomeController> logger, IEmailReaderService emailReaderService, CryptoHelper cryptoHelper
            )
        {
            _logger = logger;
            _emailReaderService = emailReaderService;
            _cryptoHelper = cryptoHelper;


        }

        public IActionResult Index()
        {
            var correos =  _emailReaderService.LeerMensajesRecibidos();
            //ncriptar UID aquí
            foreach (var correo in correos)
           {
                correo.EncryptedUid = _cryptoHelper.Encrypt(correo.Uid.ToString());
            }

            return View(correos);
        }

        public IActionResult Detalle( string uid )
        {
            string uidDesencriptado = _cryptoHelper.Decrypt(uid);
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
