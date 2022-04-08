using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


       
        public async Task<ActionResult> Basic(string tperiodo = "", int id= 0)
        {
            var hijos = "Cumplimiento desagregado";

            PeriodosController controlPeriodo = new PeriodosController(db, userManager);
            CategoriasController controlCategoria = new CategoriasController(db, userManager);

           NivelsController controlNivel = new NivelsController(db, userManager);

            List<Widget> Widgets = new List<Widget>();
           
            int Periodo = 0;
            //obtiene el periopdo por defecto si no se define 1


            if (tperiodo == "" || tperiodo == null)
            {
                var pertemp = await controlPeriodo.GetLastEnabled();
                if (pertemp == null)
                {
                    return RedirectToAction("Index", "Main");
                }
                Periodo = pertemp.id;
            }
            else
            {
                Periodo = Int32.Parse(tperiodo);
            }

            // obtener la categoria principal si no se define 1
            
            if (id == 0)
            {
                Categoria categoria = await controlCategoria.getMain();
                if (categoria != null)
                {
                    id = categoria.id;
                    var nivel = categoria.Nivel.numero + 1;

                    var nivelH = await controlNivel.getFromNumero(nivel);
                    if(nivelH != null)
                    {
                        hijos = "Cumplimiento acumulado por " + nivelH.nombre;
                    }
                }
            }
            else
            {
                Categoria categoria = await controlCategoria.getFromId(id);
                if (categoria != null)
                {
                    
                    var nivel = categoria.Nivel.numero + 1;

                    var nivelH = await controlNivel.getFromNumero(nivel);
                    if (nivelH != null)
                    {
                        hijos = "Cumplimiento acumulado por " + nivelH.nombre;
                    }
                }
            }




            Widget widget0 = new Widget();
            widget0.Ancho = "12";
            widget0.Alto = "fit-content";
            widget0.Name = "CategoriaRuta";


            Widgets.Add(widget0);



            Widget widget = new Widget();
            widget.Ancho = "4";
            widget.Alto = "calc(45% - 37px)";
            widget.Name = "CategoriaGaugaje";
            widget.Titulo = "Cumplimiento anual acumulado";
            Widgets.Add(widget);
            Widget widget2 = new Widget();
            widget2.Ancho = "8";
            widget2.Alto = "calc(45% - 37px)";
            widget2.Name = "CategoriaTrimestral"; //
            widget2.Titulo = "Histórico Anual";
            widget2.Tipo = "default";
            Widgets.Add(widget2);
            Widget widget3 = new Widget();
            widget3.Ancho = "4";
            widget3.Alto = "55%";
            widget3.Name = "CategoriaInfo";
            widget3.Titulo = "Información";
            Widgets.Add(widget3);
            Widget widget4 = new Widget();
            widget4.Ancho = "8";
            widget4.Alto = "55%";
            //widget4.Name = "IndicadoresCategoria";
            widget4.Name = "CategoriaHijosEstadoBarras";
            widget4.Titulo = hijos;
            Widgets.Add(widget4);


            ViewBag.IdCategoria = id;
            ViewBag.Periodo = Periodo;

            ViewBag.Widgets = Widgets;
            return View();
        }

    }
}
