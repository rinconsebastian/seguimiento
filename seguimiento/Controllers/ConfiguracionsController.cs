using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public class ConfiguracionsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ConfiguracionsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Index2()
        {
           return View(await db.Configuracion.ToListAsync());
        }
       

        [Authorize(Policy = "Configuracion.General")]
        public async  Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Configuracion configuracion = await  db.Configuracion.FindAsync(id);
            if (configuracion == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(configuracion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Edit(Configuracion configuracion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index2");
            }
            return View(configuracion);
        }
    }
}
