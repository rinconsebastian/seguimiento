using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{

    public class PeriodosController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public PeriodosController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<List<Periodo>> getAll()
        {
            // var periodos = db.Periodoes.SqlQuery("SELECT * FROM Periodoes WHERE tipo <> 'lineabase'").ToList();
            var periodos = await db.Periodo.Where(n => n.tipo != "lineabase").OrderBy(n => n.orden).ToListAsync();

            return periodos;
        }


        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Index()
        {

            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }

            return View(await db.Periodo.OrderBy(n => n.orden).ToListAsync());
        }

        // GET: Periodos/Edit/5
        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Edit(int id)
        {
          
            Periodo periodo = await db.Periodo.FindAsync(id);
            if (periodo == null) { return NotFound(); }

            List<SelectListItem> TipoPeriodoLista = new List<SelectListItem>();
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Linea base", Value = "lineabase" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Periodo", Value = "periodo" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Subtotal", Value = "subtotal" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "total", Value = "Total" });

            ViewBag.Tipos = new SelectList(TipoPeriodoLista, "Value", "Text", periodo.tipo);


            return View(periodo);
        }

        // POST: Periodos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Edit( Periodo periodo)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(periodo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            List<SelectListItem> TipoPeriodoLista = new List<SelectListItem>();
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Linea base", Value = "lineabase" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Periodo", Value = "periodo" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Subtotal", Value = "subtotal" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "total", Value = "Total" });

            ViewBag.tipo = new SelectList(TipoPeriodoLista, "Value", "Text", periodo.tipo);

            return View(periodo);
        }

        // GET: Periodos/Create
        [Authorize(Policy = "Periodo.Editar")]
        public  ActionResult Create()
        { 
            List<SelectListItem> TipoPeriodoLista = new List<SelectListItem>();
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Linea base", Value = "lineabase" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Periodo", Value = "periodo" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Subtotal", Value = "subtotal" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "total", Value = "Total" });

            ViewBag.Tipos = new SelectList(TipoPeriodoLista, "Value", "Text", "");


            return View();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Create(Periodo periodo)
        {
           
            if (ModelState.IsValid)
            {
                await db.Periodo.AddAsync(periodo);
                await db.SaveChangesAsync();

                EjecucionsController controlEjecucion = new EjecucionsController(db, userManager);
                await controlEjecucion.CrearFaltantes();
                return RedirectToAction("Index");
            }

            List<SelectListItem> TipoPeriodoLista = new List<SelectListItem>();
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Linea base", Value = "lineabase" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Periodo", Value = "periodo" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "Subtotal", Value = "subtotal" });
            TipoPeriodoLista.Add(new SelectListItem() { Text = "total", Value = "Total" });

            ViewBag.tipo = new SelectList(TipoPeriodoLista, "Value", "Text", "");


            return View(periodo);
        }

        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Details(int id)
        {
           
            Periodo periodo = await db.Periodo.FindAsync(id);
            if (periodo == null) { return NotFound(); }
            return View(periodo);
        }


        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> Delete(int id)
        {
            Periodo periodo = db.Periodo.Find(id);
            if (periodo == null){return NotFound();}
            return View(periodo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Periodo.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);
            Periodo periodo =await db.Periodo.FindAsync(id);
            try
            {
                db.Periodo.Remove(periodo);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = controlConfiguracion.SqlErrorHandler(ex);
                HttpContext.Session.SetComplex("error", error);
            }
            return RedirectToAction("Index");
        }


        public async  Task<Periodo> GetLastEnabled()
        {
            Periodo respuesta =await db.Periodo.Where(m => m.cargado == true && (m.tipo == "subtotal" || m.tipo == "total")).OrderByDescending(m => m.orden).FirstOrDefaultAsync();
            if (respuesta == null)
            {
                respuesta = await db.Periodo.Where(m => m.tipo == "subtotal").OrderBy(m => m.orden).FirstOrDefaultAsync();
            }


            return respuesta;
        }

        public async Task<Periodo> GetFromId(int id)
        {
            Periodo respuesta =await db.Periodo.FindAsync(id);
            return respuesta;
        }

        public async Task<List<Periodo>> PeriodosFromSubtotal(Periodo subtotal)
        {
            List<Periodo> respuesta = new List<Periodo>();

            int ajusteSubtotal = 0;

            if (subtotal.tipo == "subtotal") { ajusteSubtotal += 1; }

            if (subtotal.orden > 1)
            {
                var periodon =await db.Periodo.Where(m => m.orden == subtotal.orden - ajusteSubtotal && m.tipo == "periodo").FirstOrDefaultAsync();
                if (periodon != null)
                {


                    while (periodon.orden >= 1 && periodon.tipo == "periodo")
                    {
                        respuesta.Add(periodon);

                        periodon = await db.Periodo.Where(m => m.orden == periodon.orden - 1 && periodon.tipo == "periodo").FirstOrDefaultAsync();
                        if (periodon == null)
                        {
                            break;
                        }

                    }
                }
            }

            return respuesta;

        }

        public async Task<List<Periodo>> PeriodosFromTotal(Periodo total)
        {
            List<Periodo> respuesta = new List<Periodo>();

            int ajusteSubtotal = 0;

            if (total.tipo == "total") { ajusteSubtotal += 1; }

            if (total.orden > 1)
            {
                var periodon = await db.Periodo.Where(m => m.orden == total.orden - ajusteSubtotal && m.tipo == "subtotal").FirstOrDefaultAsync();
                if (periodon != null)
                {


                    while (periodon.orden >= 1 && periodon.tipo == "subtotal")
                    {
                        respuesta.Add(periodon);

                        periodon = await db.Periodo.Where(m => (m.orden <= periodon.orden - 1) && m.tipo == "subtotal").OrderByDescending(m => m.orden).FirstOrDefaultAsync();
                        if (periodon == null)
                        {
                            break;
                        }

                    }
                }
            }

            return respuesta;

        }

        public async Task<int> GetSubtotalFromPeriodo(int idPeriodo)
        {
            var r = idPeriodo;

            var periodo = await db.Periodo.Where(n => n.id == idPeriodo).FirstOrDefaultAsync();
            if (periodo != null)
            {
                var subtotal = await db.Periodo.Where(n => n.tipo == "subtotal" && n.orden >= periodo.orden).OrderBy(n => n.orden).FirstOrDefaultAsync();

                if (subtotal != null)
                {
                    r = subtotal.id;
                }

            }
            return r;
        }

    }
}
