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
    public class NIvelsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NIvelsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<Nivel> getMain()
        {
            Nivel nivel = await db.Nivel.OrderBy(n=>n.numero).FirstOrDefaultAsync();
            return nivel;
        }

        public async Task<Nivel> getFromNumero(int numero)
        {
            Nivel nivel = await db.Nivel.Where(n=>n.numero == numero).FirstOrDefaultAsync();
            return nivel;
        }
    }
}
