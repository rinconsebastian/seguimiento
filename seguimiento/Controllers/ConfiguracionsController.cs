using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
        public class ConfiguracionsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ConfiguracionsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            //otificationController controlNotificaciones = new NotificationController();

            Sesion sesion = HttpContext.Session.Get<Sesion>("sesion");
            if (sesion == null)
            {
                return RedirectToAction("Index", "Main");
            }
            //controlNotificaciones.ToUser(sesion.usuario, "ingreso al panel de configuación", "<h2>texto prueba</h2>");

            return View();
        }
    }
}
