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
    public class CategoriaHijosEstadoBarrasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriaHijosEstadoBarrasViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
        {
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);
            ViewBag.indicadores = false;

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);

            List<EjecucionCategoria> ejecuciones = await controlEjecucionCategoria.GetHijosFromCatIDPerID(IdCategoria, IdPeriodo);

            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;

            if (ejecuciones.Count == 0)
            {
                ViewBag.indicadores = true;
                ViewBag.itemId = IdCategoria;
            }

            return View(ejecuciones);

        }
    }
}
