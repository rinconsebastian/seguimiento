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
    public class UsersByRoleViewComponent : ViewComponent
    {
    
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersByRoleViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string idRole)
        {
            var items = await GetItemsAsync(idRole);
            return View(items);
        }
        private Task<List<ApplicationUser>> GetItemsAsync(string idRole)
        {
            var ids = db.UserRoles.Where(n => n.RoleId == idRole).Select(n => n.UserId).ToList();
            return db.Users.Where(n => ids.Contains(n.Id)).ToListAsync();
        }
    }
}
