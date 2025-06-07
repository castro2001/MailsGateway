using Application.DTO;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace MailGateway.Controllers.Seguridad
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _context;

        public AccesoController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }

        [HttpGet]

        public IActionResult Login()
        {
            //if()
            return View();
        }

        

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login( UsuarioDTO usuarioDTO)
        {
            //if()
            return View();
        }


        [HttpPost]
        public IActionResult Registro(UsuarioDTO usuarioDTO)
        {
            //if()
            return View();
        }

    }
}
