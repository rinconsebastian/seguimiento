using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Formulas;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class WidgetController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public WidgetController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

       
        
        // GET: Widget
        public async Task<ActionResult> CategoriaGaugaje(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {
            var Numero = Int32.Parse(numero);
            var Id = Int32.Parse(id);
            var periodot = Int32.Parse(periodo);

            // var numero = Int32.Parse(Request["posicion"]);
            // var id = Int32.Parse(Request["id"]);
            // var Periodo = Int32.Parse(Request["periodo"]);

            EjecucionCategoria ejecucion = await db.EjecucionCategoria.Where(n => n.IdCategoria == Id && n.idperiodo == periodot).FirstOrDefaultAsync();

            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.tipo = tipo;
            return View(ejecucion);
        }


        public async Task<ActionResult> CategoriaHistoricoTotal(String numero, String id)

        {  
            var Numero = Int32.Parse(numero);
            var Id = Int32.Parse(id);


            // var numero = Int32.Parse(Request["posicion"]);
            // var id = Int32.Parse(Request["id"]);
            // var Periodo = Int32.Parse(Request["periodo"]);

            List<EjecucionCategoria> ejecuciones = await db.EjecucionCategoria.Where(n => n.IdCategoria == Id).ToListAsync();

            ViewBag.Numero = Numero;
            return View(ejecuciones);
        }

        public async Task<ActionResult> CategoriaRuta(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {

            CategoriasController controlCategoria = new CategoriasController(db, userManager);

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);

            List<Categoria> Categorias = new List<Categoria>();


            Categoria categoria = await controlCategoria.getFromId(IdCategoria);
            Categorias.Add(categoria);

            while (categoria.CategoriaPadre != null)
            {
                Categorias.Add(categoria.CategoriaPadre);
                categoria = await controlCategoria.getFromId(categoria.CategoriaPadre.id);
            }



            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            Categorias.Reverse();
            ViewBag.tipo = tipo;

            //listado de periodos
            ViewBag.Periodos = new SelectList(await db.Periodo.Where(n => n.tipo == "periodo" || n.tipo == "subtotal" || n.tipo == "Total").ToListAsync(), "id", "nombre", System.Convert.ToInt32(periodo));

            return View(Categorias);


        }

        public async Task<ActionResult> CategoriaTrimestral(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);
            EvaluacionsController controlEvaluacion = new EvaluacionsController(db);


            var Numero = Int32.Parse(numero);
            var Id = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);

            var periodoO = await db.Periodo.Where(n => n.id == IdPeriodo).FirstOrDefaultAsync();


            List<Object> respuesta = new List<object>();

            List<EjecucionCategoria> datos = new List<EjecucionCategoria>();
            // var numero = Int32.Parse(Request["posicion"]);
            // var id = Int32.Parse(Request["id"]);
            // var Periodo = Int32.Parse(Request["periodo"]);

            if (periodoO.tipo == "Total")
            {
                datos = await controlEjecucionCategoria.GetFromCategoriaYTotal(Id, IdPeriodo);

            }
            else
            {
                datos = await controlEjecucionCategoria.GetFromCategoriaYSubtotal(Id, IdPeriodo);

            }

            //======================================== obtiene los semanoforos===============================
            List<Evaluacion> evaluaciones = await controlEvaluacion.Get(Id, "Categotia");

            var semaforos =  controlEvaluacion.SetEvaluacionCategoria(datos, evaluaciones);

            //======================================== obtiene los semanoforos===============================
            int ndato = 0;
            foreach (EjecucionCategoria dato in datos)
            {
                object[] EjecucionconSemaforo = { dato, semaforos[ndato] };
                respuesta.Add(EjecucionconSemaforo);
                ndato++;
            }


            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;
            ViewBag.Ejecuciones = respuesta;
            return View();
        }

        public async Task<ActionResult> CategoriaHijosEstadoBarras(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);

            List<EjecucionCategoria> ejecuciones =await controlEjecucionCategoria.GetHijosFromCatIDPerID(IdCategoria, IdPeriodo);

            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;

            if (ejecuciones.Count == 0)
            {

                ActionResult respuesta = await IndicadoresCategoria(numero, id, periodo, alto, ancho, titulo, tipo);
                return respuesta;
            }

            return View(ejecuciones);


        }
        public async Task<ActionResult> IndicadoresCategoria(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {
            ConfiguracionsController controlConfiguración = new ConfiguracionsController(db, userManager);

            Configuracion config = await controlConfiguración.Get();

            var Numero = Int32.Parse(numero);
            string IdCategoria = id;
            var IdPeriodo = Int32.Parse(periodo);




            ViewBag.id = IdCategoria;
            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;


            return View("~/Views/Widget/IndicadoresCategoria.cshtml");


        }

        public async Task<ActionResult> CategoriaInfo(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)

        {
            ConfiguracionsController controlConfiguración = new ConfiguracionsController(db, userManager);
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);
            PeriodosController controlPeriodo = new PeriodosController(db, userManager);


            Configuracion config = await controlConfiguración.Get();

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);




            Categoria categoria =await controlCategoria.getFromId(IdCategoria);

            //hijos
            var hijos = await controlCategoria.getFromCategoria(categoria.id);

            if (hijos.Count > 0)
            {
                ViewBag.nombreHijos = hijos[0].Nivel.nombre;
                ViewBag.numeroHijos = hijos.Count;
            }



            //ponderador
            if (config.PonderacionTipo == "PonderacionAbsoluta")
            {
               

                PonderacionAbsoluta pond = new PonderacionAbsoluta(db, userManager);
                categoria.Ponderador =await  pond.CategoriaPonderador(categoria.id); //80
            }

            //indicadores
            var indi = await controlIndicador.getFromCategoria(categoria.id);
            var indicadores = indi.Where(n => n.ponderador > 0);
            if (indicadores.Count() > 0)
            {
                ViewBag.numeroIndicadores = indicadores.Count();
            }
            else
            {
                ViewBag.numeroIndicadores = await controlCategoria.NumeroIndicadores(categoria.id); //81
            }

            //numero de indicadores en cada evaluacion del último periodo reportado
            if (IdPeriodo != 0)
            {
                // var LastPeriodoReport = COntrolPeriodos.UltimoPeriodoRepFromSubtotal(IdPeriodo);

                ViewBag.periodo = await controlPeriodo.GetFromId(IdPeriodo);
                var periodoSubtotal = await controlPeriodo.GetSubtotalFromPeriodo(IdPeriodo);
                if (periodoSubtotal > 0)
                {
                    ViewBag.periodoSubtotal = await db.Periodo.FindAsync(periodoSubtotal);

                }
            }




            ViewBag.Numero = Numero;
            ViewBag.alto = alto;
            ViewBag.ancho = ancho;
            ViewBag.titulo = titulo;
            //ViewBag.periodo = periodo;
            ViewBag.tipo = tipo;


            return View(categoria);


        }
    }
}
