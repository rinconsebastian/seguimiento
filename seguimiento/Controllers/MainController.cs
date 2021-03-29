using Microsoft.AspNetCore.Mvc;
using seguimiento.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class MainController : Controller
    {
        private readonly ApplicationDbContext db;

        public MainController(ApplicationDbContext context)
        {
            db = context;
        }


        public IActionResult Index(int? categoriaid)
        {
           
            ViewBag.idInicial = categoriaid;

            ViewBag.ClaseContainer = db.Configuracion.First().EstiloReporte;

            return View();
        }
    }
}
