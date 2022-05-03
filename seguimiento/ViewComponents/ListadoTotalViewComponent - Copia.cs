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
    public class ListadoTotalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public ListadoTotalViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }
        private  Task<List<Categoria>> GetItemsAsync()
        {

            return  db.Categoria.Include(c => c.Nivel).OrderBy(n => n.numero).ToListAsync();
           

        }
    }
}
