using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Controllers;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class IndicadoresCategoriaViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public IndicadoresCategoriaViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
        {
            ConfiguracionsController controlConfiguración = new ConfiguracionsController(db, userManager);

            Configuracion config = await controlConfiguración.Get();

            var Numero = Int32.Parse(numero);
            string IdCategoria = id;
            var IdPeriodo = Int32.Parse(periodo);




            ViewBag.id = IdCategoria;
            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;


            return View("~/Views/Shared/IndicadoresCategoria/Default.cshtml");

        }
    }
}
