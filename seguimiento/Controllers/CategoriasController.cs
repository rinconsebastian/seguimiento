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
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriasController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


        [Authorize(Policy = "Configuracion.Responsable")]
        public async Task<ActionResult> GetFromResponsable(int idResponsable)
        {


            var categorias =await  db.Categoria.Where(n => n.IdResponsable == idResponsable).OrderBy(n => n.numero).ToListAsync();

            return View(categorias);
        }

    }
}
