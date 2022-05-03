using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Controllers;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class CategoriaTrimestralViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriaTrimestralViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
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

            var semaforos = controlEvaluacion.SetEvaluacionCategoria(datos, evaluaciones);

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
    }
}
