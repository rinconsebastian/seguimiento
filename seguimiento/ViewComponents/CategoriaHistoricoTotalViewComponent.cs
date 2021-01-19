using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class CategoriaHistoricoTotalViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriaHistoricoTotalViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
        {
            var Numero = Int32.Parse(numero);
            var Id = Int32.Parse(id);


            // var numero = Int32.Parse(Request["posicion"]);
            // var id = Int32.Parse(Request["id"]);
            // var Periodo = Int32.Parse(Request["periodo"]);

            List<EjecucionCategoria> ejecuciones = await db.EjecucionCategoria.Where(n => n.IdCategoria == Id).ToListAsync();

            ViewBag.Numero = Numero;
            return View(ejecuciones);
        }
    }
}
