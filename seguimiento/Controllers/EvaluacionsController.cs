using Microsoft.AspNetCore.Authorization;
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
    public class EvaluacionsController : Controller
    {
        private readonly ApplicationDbContext db;


        public EvaluacionsController(ApplicationDbContext context)
        {
            db = context;

        }


        //==================================================== IDENTIFICA LA EVALUACION QUE LE CORRESPONDE A UN ELEMENTO
        public async Task<List<Evaluacion>> Get(int Id, string elemento)
        {
            List<Evaluacion> evaluaciones = new List<Evaluacion>();
            bool defecto = false;
            bool error = false;

            switch (elemento)
            {
                case "Categoria":
                    evaluaciones = await db.Evaluacion.Where(n => n.Contexto == "Categoria" && n.Categoria.id == Id).OrderBy(n => n.Minimo).ToListAsync();
                    if (evaluaciones.Count() < 1)
                    { defecto = true; }
                    break;
                case "Indicador":
                    evaluaciones = await db.Evaluacion.Where(n => n.Contexto == "Indicador" && n.Indicador.id == Id).OrderBy(n => n.Minimo).ToListAsync();
                    if (evaluaciones.Count() < 1)
                    { defecto = true; }
                    break;
                default:
                    evaluaciones = await db.Evaluacion.Where(n => n.Contexto == "Global").OrderBy(n => n.Minimo).ToListAsync();
                    if (evaluaciones.Count() < 1)
                    { error = true; }
                    break;
            }

            if (defecto == true)
            {
                evaluaciones = await db.Evaluacion.Where(n => n.Contexto == "Global").OrderBy(n => n.Minimo).ToListAsync();
                if (evaluaciones.Count() < 1)
                { error = true; }
            }
            if (error == true)
            {
                Evaluacion eval = new Evaluacion();
                eval.Contexto = "Global";
                eval.Color = "#FFFFFF";
                eval.Minimo = 0;
                eval.Maximo = 100;
                eval.Nombre = "NA";
                evaluaciones.Add(eval);
            }

            return evaluaciones;
        }

        public EjecucionCalculada SetEvaluacion(EjecucionCalculada ejecucion, List<Evaluacion> semaforos)
        {
            EvaluacionDisplay eval = new EvaluacionDisplay();
            eval.Color = "";
            eval.texto = "";
            if ((ejecucion.EjecutadoError == null && ejecucion.PlaneadoError == null) || ejecucion.Periodo.tipo != "periodo")
            {

                foreach (var semaforox in semaforos)
                {
                    if (ejecucion.Calculado >= semaforox.Minimo && ejecucion.Calculado <= semaforox.Maximo)
                    {
                        eval.Color = semaforox.Color;
                        eval.texto = semaforox.Nombre;
                    }
                }
            }
            ejecucion.Evaluacion = eval;
            return ejecucion;
        }

        public List<EvaluacionDisplay> SetEvaluacionCategoria(List<EjecucionCategoria> ejecuciones, List<Evaluacion> semaforos)
        {
            List<EvaluacionDisplay> respuesta = new List<EvaluacionDisplay>();

            foreach (EjecucionCategoria ejecucionx in ejecuciones)
            {

                EvaluacionDisplay eval = new EvaluacionDisplay();
                eval.Color = "";
                eval.texto = "";
                if (ejecucionx.Calculado >= 0)
                {

                    foreach (var semaforox in semaforos)
                    {
                        if (ejecucionx.Calculado >= semaforox.Minimo && ejecucionx.Calculado <= semaforox.Maximo)
                        {
                            eval.Color = semaforox.Color;
                            eval.texto = semaforox.Nombre;
                        }
                    }
                }
                respuesta.Add(eval);
            }
            return respuesta;
        }

        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Index()
        {
            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }
            var evaluacions = await db.Evaluacion.ToListAsync();
            return View(evaluacions);
        }


        [Authorize(Policy = "Evaluacion.Editar")]
        public ActionResult Details(int id)
        {
            Evaluacion evaluacion = db.Evaluacion.Find(id);
            if (evaluacion == null) { return NotFound(); }
            return View(evaluacion);
        }

        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Create()
        {
            //lista de contextos
            List<SelectListItem> ContextosLista = new List<SelectListItem>();
            ContextosLista.Add(new SelectListItem() { Text = "Global", Value = "Global" });
            ContextosLista.Add(new SelectListItem() { Text = "Categoria", Value = "Categoria" });
            ContextosLista.Add(new SelectListItem() { Text = "Indicador", Value = "Indicador" });
            ViewBag.Contexto = new SelectList(ContextosLista, "Value", "Text", "");

            //Lista de categorias
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", "");

            //lista de indicadores
            List<SelectListItem> IndicadoresLista = new List<SelectListItem>();
            var IndicadoresListadb = await db.Indicador.ToListAsync();
            foreach (var itemn in IndicadoresListadb)
            {
                IndicadoresLista.Add(new SelectListItem() { Text = itemn.codigo + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Indicadores = new SelectList(IndicadoresLista, "Value", "Text", "");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Create(Evaluacion evaluacion)
        {
            var idCategoria = HttpContext.Request.Form["Categoria"].ToString();
            var idIndicador = HttpContext.Request.Form["Indicador"].ToString();
            if (idCategoria != null && idCategoria != "")
            {
                var categoria = await db.Categoria.FindAsync(Int32.Parse(idCategoria));
                evaluacion.Categoria = categoria;
            }
            if (idIndicador != null && idIndicador != "")
            {
                var Indicador = await db.Indicador.FindAsync(Int32.Parse(idIndicador));
                evaluacion.Indicador = Indicador;
            }

            if (ModelState.IsValid)
            {
                db.Evaluacion.Add(evaluacion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //lista de contextos
            List<SelectListItem> ContextosLista = new List<SelectListItem>();
            ContextosLista.Add(new SelectListItem() { Text = "Global", Value = "Global" });
            ContextosLista.Add(new SelectListItem() { Text = "Categoria", Value = "Categoria" });
            ContextosLista.Add(new SelectListItem() { Text = "Indicador", Value = "Indicador" });
            ViewBag.Contexto = new SelectList(ContextosLista, "Value", "Text", "");

         
            //Lista de categorias
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", "");

            //lista de indicadores
            List<SelectListItem> IndicadoresLista = new List<SelectListItem>();
            var IndicadoresListadb = await db.Indicador.ToListAsync();
            foreach (var itemn in IndicadoresListadb)
            {
                IndicadoresLista.Add(new SelectListItem() { Text = itemn.codigo + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Indicadores = new SelectList(IndicadoresLista, "Value", "Text", "");

            return View(evaluacion);
        }

        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Edit(int id)
        {
            Evaluacion evaluacion = await db.Evaluacion.FindAsync(id);
            if (evaluacion == null){return NotFound();}

            if(evaluacion.Categoria != null) { evaluacion.IdCategoria = evaluacion.Categoria.id; }
            if (evaluacion.Indicador != null) { evaluacion.IdIndicador = evaluacion.Indicador.id; }

            //lista de contextos
            List<SelectListItem> ContextosLista = new List<SelectListItem>();
            ContextosLista.Add(new SelectListItem() { Text = "Global", Value = "Global" });
            ContextosLista.Add(new SelectListItem() { Text = "Categoria", Value = "Categoria" });
            ContextosLista.Add(new SelectListItem() { Text = "Indicador", Value = "Indicador" });
            ViewBag.Contexto = new SelectList(ContextosLista, "Value", "Text", evaluacion.Contexto);

            //Lista de categorias
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", evaluacion.IdCategoria);

            //lista de indicadores
            List<SelectListItem> IndicadoresLista = new List<SelectListItem>();
            var IndicadoresListadb = await db.Indicador.ToListAsync();
            foreach (var itemn in IndicadoresListadb)
            {
                IndicadoresLista.Add(new SelectListItem() { Text = itemn.codigo + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Indicadores = new SelectList(IndicadoresLista, "Value", "Text", evaluacion.IdIndicador);

            return View(evaluacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Edit(Evaluacion evaluacion)
        {
            Categoria categoria = null;
            if (evaluacion.IdCategoria != 0)
            {
                categoria = await db.Categoria.FindAsync(evaluacion.IdCategoria);
                evaluacion.Categoria = categoria;
            }
            else
            {
                evaluacion.Categoria = null;
            }
            

            Indicador indicador = null;
            if (evaluacion.IdIndicador != 0)
            {
                indicador = await db.Indicador.FindAsync(evaluacion.IdIndicador);
                evaluacion.Indicador = indicador;
            }
            else
            {
                evaluacion.Indicador= null;
            }
            

            if (ModelState.IsValid)
            {
                db.Entry(evaluacion).State = EntityState.Modified;

                if(evaluacion.Categoria == null)
                {
                   // db.Database.ExecuteSqlRaw("UPDATE `evaluacion` SET `Categoriaid` = null WHERE `Id` = {0};", evaluacion.Id);
                }

                if (evaluacion.Indicador == null)
                {
                    //db.Database.ExecuteSqlRaw("UPDATE `evaluacion` SET `Indicadorid` = null WHERE `Id` = {0};", evaluacion.Id);
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            //lista de contextos
            List<SelectListItem> ContextosLista = new List<SelectListItem>();
            ContextosLista.Add(new SelectListItem() { Text = "Global", Value = "Global" });
            ContextosLista.Add(new SelectListItem() { Text = "Categoria", Value = "Categoria" });
            ContextosLista.Add(new SelectListItem() { Text = "Indicador", Value = "Indicador" });
            ViewBag.Contexto = new SelectList(ContextosLista, "Value", "Text", evaluacion.Contexto);

            //Lista de categorias
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", evaluacion.IdCategoria);

            //lista de indicadores
            List<SelectListItem> IndicadoresLista = new List<SelectListItem>();
            var IndicadoresListadb = await db.Indicador.ToListAsync();
            foreach (var itemn in IndicadoresListadb)
            {
                IndicadoresLista.Add(new SelectListItem() { Text = itemn.codigo + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Indicadores = new SelectList(IndicadoresLista, "Value", "Text", evaluacion.IdIndicador);

            return View(evaluacion);
        }

        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> Delete(int id)
        {
            Evaluacion evaluacion = await db.Evaluacion.FindAsync(id);
            if (evaluacion == null) { return NotFound(); }
            return View(evaluacion);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Evaluacion.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Evaluacion evaluacion = await db.Evaluacion.FindAsync(id);
            db.Evaluacion.Remove(evaluacion);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
