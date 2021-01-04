using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index(int? categoriaid)
        {
           
            ViewBag.idInicial = categoriaid;
            return View();
        }
    }
}
