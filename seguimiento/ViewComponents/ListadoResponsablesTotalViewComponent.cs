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
    public class ListadoResponsablesTotalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public ListadoResponsablesTotalViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }
        private async Task<List<Responsable>> GetItemsAsync()
        {
            List<Responsable> r = new List<Responsable>();

            var r0= await db.Responsable.Where(n=>n.Categorias.Count > 0).OrderBy(n=>n.Nombre).ToListAsync();

            if (r0.Count > 0) {
                var r1 = r0.Where(n => n.Categorias.First().Indicadores.Count > 0).ToList();
                if (r1.Count > 0)
                {
                    r.AddRange(r1);
                }
            }

            return r;

        }
    }
}
