using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class ResponsablesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ResponsablesController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        [Authorize(Policy = "Configuracion.Responsable")]
        public async Task<IActionResult> Index2()
        {
            
            var responsables = await db.Responsable.Where(n => n.IdJefe == 0).ToListAsync();
            return View(responsables);
        }
    }
}
