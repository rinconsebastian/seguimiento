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
    public class CategoriasFromResponsableViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public CategoriasFromResponsableViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idResponsable)
        {
            var items = await GetItemsAsync(idResponsable);
            return View(items);
        }
        private Task<List<Categoria>> GetItemsAsync(int idResponsable)
        {

           return db.Categoria.Where(n => n.IdResponsable == idResponsable).OrderBy(n => n.numero).ToListAsync();
            
        }

    }
}
