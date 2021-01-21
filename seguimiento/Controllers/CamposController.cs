using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using seguimiento.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace seguimiento.Controllers
{
    public class CamposController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CamposController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }
 

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<bool> BorrarDeIndicador(int id)
        {
            bool error = true;
            var valorCampo = await db.ValorCampo.Where(n => n.IndicadorPadre.id == id).ToListAsync();

            try
            {
                db.RemoveRange(valorCampo);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = false;
            }



            return error;

        }


        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Campo.ToListAsync());
        }

        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Details(int id)
        {
            Campo campo = await db.Campo.FindAsync(id);
            return View(campo);
        }

        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", "");
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Create(Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Campo.Add(campo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", "");
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", "");
            return View(campo);
        }

        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Edit(int id)
        {

            Campo campo = await db.Campo.FindAsync(id);

            if (campo.NivelPadre != null)
            {
                ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", campo.NivelPadre.id);
            }
            else
            {
                ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", "");
            }

            if (campo.TipoIndicadorPadre != null)
            {
                ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", campo.TipoIndicadorPadre.Id);
            }
            else
            {
                ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", "");
            }

            return View(campo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Edit(Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (campo.NivelPadre != null)
            {
                ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", campo.NivelPadre.id);
            }
            else
            {
                ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", "");
            }

            if (campo.TipoIndicadorPadre != null)
            {
                ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", campo.TipoIndicadorPadre.Id);
            }
            else
            {
                ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", "");
            }
            return View(campo);
        }
        
        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> Delete(int? id)
        {
            Campo campo = await db.Campo.FindAsync(id);
            return View(campo);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Campo.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {  
            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            Campo campo = await db.Campo.FindAsync(id);
            try
            {
                db.Campo.Remove(campo);
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

