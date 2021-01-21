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
    public class NivelsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public NivelsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
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

        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Index()
        {
            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }

            return View(await db.Nivel.OrderBy(n => n.numero).ToListAsync());
        }

        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Details(int id)
        {
            Nivel nivel = await db.Nivel.FindAsync(id);
            return View(nivel);
        }

        [Authorize(Policy = "Nivel.Editar")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Create( Nivel nivel)
        {
            if (ModelState.IsValid)
            {
                db.Nivel.Add(nivel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nivel);
        }

        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Edit(int id)
        {
            Nivel nivel = await db.Nivel.FindAsync(id);
            return View(nivel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Edit(Nivel nivel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nivel);
        }

        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> Delete(int id)
        {
            Nivel nivel = await db.Nivel.FindAsync(id);
            return View(nivel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Nivel.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db,userManager);
            Nivel nivel = await db.Nivel.FindAsync(id);
            try
            {
                db.Nivel.Remove(nivel);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = controlConfiguracion.SqlErrorHandler(ex);
                HttpContext.Session.SetComplex("error", error);
            }
            return RedirectToAction("Index");
        }
    }
}
