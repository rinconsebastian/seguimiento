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
    public class NotasController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NotasController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;

        }


        public async Task<int> NumeroNotasEstadoCategoriaId(int id, string estado)
        {
            var numero = await db.Nota.Where(n => n.Estado == estado && n.IdCategoria == id).CountAsync();
                
              //Database.SqlQuery<int>("SELECT COUNT(*) FROM Notas WHERE Estado= '" + estado + "' AND IdCategoria= " + id).First();

            return numero;
        }
    }
}
