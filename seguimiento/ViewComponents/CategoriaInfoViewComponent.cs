using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Controllers;
using seguimiento.Data;
using seguimiento.Formulas;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.ViewComponents
{
    public class CategoriaInfoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriaInfoViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(String numero, String id, String periodo, string alto, string ancho, string titulo, string tipo)
        {
            ConfiguracionsController controlConfiguración = new ConfiguracionsController(db, userManager);
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);
            PeriodosController controlPeriodo = new PeriodosController(db, userManager);


            Configuracion config = await controlConfiguración.Get();

            var Numero = Int32.Parse(numero);
            var IdCategoria = Int32.Parse(id);
            var IdPeriodo = Int32.Parse(periodo);




            Categoria categoria = await controlCategoria.getFromId(IdCategoria);

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
                categoria.Ponderador = await pond.CategoriaPonderador(categoria.id); //80
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
