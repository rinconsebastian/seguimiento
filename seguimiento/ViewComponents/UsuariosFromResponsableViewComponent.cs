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
    public class UsuariosFromResponsableViewComponent : ViewComponent
    {
    
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UsuariosFromResponsableViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idResponsable)
        {
            var items = await GetItemsAsync(idResponsable);
            return View(items);
        }
        private Task<List<ApplicationUser>> GetItemsAsync(int idResponsable)
        {

            return db.Users.Where(n => n.IDDependencia == idResponsable).OrderBy(n => n.Nombre).ToListAsync();

        }
    }
}
