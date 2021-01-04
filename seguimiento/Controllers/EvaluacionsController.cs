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
    }
}
