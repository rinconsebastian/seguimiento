using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Formulas;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class EjecucionsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public EjecucionsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }



        public async Task<IActionResult> DetailsPop(int id, string tipo="", string mensaje="")
        {
            ConfiguracionsController configuracionControl = new ConfiguracionsController(db, userManager);
            EvaluacionsController controlEvaluacion = new EvaluacionsController(db);
            UnidadesdeMedida Unidad = new UnidadesdeMedida();

            EjecucionCalculada respuestaFormato = new EjecucionCalculada();

            Ejecucion ejec = await db.Ejecucion.Where(n=>n.id == id).FirstOrDefaultAsync();
            if (ejec != null)
            {

                //-------------------------------------------------------identificar si un usuario tiene acceso a editar una ejecucion



                if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1") ||
                                        (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1")))
                {
                    ViewBag.MostrarBotonEditarEjecucion = await configuracionControl.PermisoMostrarEditarEjecucion(User, ejec);
                }
                else
                {
                    ViewBag.MostrarBotonEditarEjecucion = false;
                }
                //-------------------------------------------------------identificar si un usuario tiene acceso a editar una ejecucion


                EjecucionCalculada respuesta = new EjecucionCalculada();


                //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
                ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + ejec.Indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                Object op = manejador.Unwrap();
                Type t = op.GetType();
                MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo

                Periodo periodoLineaBase = db.Periodo.Where(n => n.tipo == "lineabase").FirstOrDefault();
                Ejecucion lineaBase = db.Ejecucion.Where(n => n.idperiodo == periodoLineaBase.id && n.idindicador == ejec.idindicador).FirstOrDefault();


                List<Evaluacion> evaluaciones = await controlEvaluacion.Get(ejec.idindicador, "Indicador");

                object[] args = { ejec, null }; //carga los argumentos en un objeto 
                var respuesta2 = (EjecucionCalculada)operadorPeriodo.Invoke(op, args); //envia los argumentos mediante invoke al metodo Calculo_periodo
                respuesta = respuesta2;
                respuestaFormato = controlEvaluacion.SetEvaluacion(Unidad.Formato(respuesta), evaluaciones); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida


                ViewBag.Adjuntos = await db.EjecucionAdjunto.Where(n => n.idejecucion == ejec.id).ToListAsync();
                ViewBag.tipo = tipo;
                ViewBag.mensaje = mensaje;

            }
            return View(respuestaFormato);
        }



        // GET: ejecucions/Edit/5
        [Authorize(Policy = "Ejecucion.Editar")]

        public async Task<ActionResult> EditPop(int id)
        {
            ConfiguracionsController configuracionControl = new ConfiguracionsController(db, userManager);
            Ejecucion ejecucion = await db.Ejecucion.Where(n=>n.id == id).FirstOrDefaultAsync();
            

            //-------------------------------------------------------identificar si un usuario tiene acceso a editar una ejecucion
           
            if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1") ||
                                        (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1")))
            {
                ViewBag.PuedeEditarEjecucion = await configuracionControl.PermisoEditarEjecucion(User, ejecucion);
                ViewBag.PuedeEditarPlaneado = await configuracionControl.PermisoEditarEjecucionPlaneado(User, ejecucion);

            }

            ViewBag.Adjuntos = await db.EjecucionAdjunto.Where(n => n.idejecucion == ejecucion.id).ToListAsync();
            return View(ejecucion);
        }

        // GET: ejecucions/Edit/5
        [Authorize(Policy = "Ejecucion.Editar")]

        public async Task<ActionResult> EditPopAdjuntos(int idEjecucion)
        {
            var adjuntos = await db.EjecucionAdjunto.Where(n => n.idejecucion == idEjecucion).ToListAsync();
            return View(adjuntos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Ejecucion.Editar")]
        public async Task<bool> EditPop(Ejecucion ejecucion)
        {
            LogsController Logg = new LogsController(db, userManager);

            bool respuesta = false;

            if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1") ||
                                       (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1")))
            {
                //ejecucion.Indicador = db.Indicadors.Find(ejecucion.idindicador);
                //ejecucion.Periodo = db.Periodoes.Find(ejecucion.idperiodo);
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    //Logg.CambioModelo("Ejecucion", "Edit", ejecucion, User.Identity.Name); //registro log

                    // db.ejecucions.Attach(ejecucion);
                    db.Entry(ejecucion).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    

                    respuesta = true;
                }
            }
            return respuesta;
        }

        public async Task<List<Ejecucion>> getFromIndicador(int idIndicador)
        {
            List<Ejecucion> ejecuciones = await db.Ejecucion.Where(n => n.idindicador == idIndicador).OrderBy(n=>n.Periodo.orden).ToListAsync();
                
               
            return ejecuciones;
        }

        public async Task<bool> CrearEjecucionesDeIndicador(Indicador indicador)
        {
            bool error = false;
            List<Periodo> periodos = await db.Periodo.ToListAsync();
            foreach (var periodo in periodos)
            {
                Ejecucion ejecucionn = new Ejecucion();
                ejecucionn.idindicador = indicador.id;
                ejecucionn.idperiodo = periodo.id;
                ejecucionn.planeado = "";
                ejecucionn.ejecutado = "";
                ejecucionn.Nota = "";
                ejecucionn.adjunto = "";
                ejecucionn.cargado = false;


                if (ModelState.IsValid)
                {
                    try
                    {
                       await db.Ejecucion.AddAsync(ejecucionn);
                       await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                    }
                }
            }

            return error;

        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<bool> BorrarEjecucionesDeIndicador(int id)
        {
            bool error = true;
            var ejecuciones = await db.Ejecucion.Where(n => n.idindicador == id).ToListAsync();

                    try
                    {
                db.RemoveRange(ejecuciones);
                await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        error = false;
                    }
                
            

            return error;

        }

        public async Task<bool> CrearFaltantes()
        {
            PeriodosController PeriodoControl = new PeriodosController(db,userManager);
            IndicadorsController IndicadorControl = new IndicadorsController(db, userManager);

            var periodos = await PeriodoControl.getAll();
            var indicadores =await IndicadorControl.GetAll();
            bool respuesta = false;
            foreach (Periodo periodo in periodos)
            {
                foreach (Indicador indicador in indicadores)
                {
                    Ejecucion ejecucionxy = await GetFromIndicadorPeriodo(indicador.id, periodo.id);
                    if (ejecucionxy == null)
                    {
                        Ejecucion ejecucionnew = new Ejecucion();
                        ejecucionnew.idindicador = indicador.id;
                        ejecucionnew.idperiodo = periodo.id;

                        await db.Ejecucion.AddAsync(ejecucionnew);
                        await db.SaveChangesAsync();

                        respuesta = true;
                    }
                }
            }

            return respuesta;
        }

        public async Task<Ejecucion> GetFromIndicadorPeriodo(int idIndicador, int idPeriodo)
        {

            //var respuesta = db.ejecucions.Where(n => n.idindicador == idIndicador && n.idperiodo == idPeriodo).ToList();
            var respuesta = await db.Ejecucion.Where(n => n.idindicador == idIndicador && n.idperiodo == idPeriodo).FirstOrDefaultAsync();
               
            return respuesta;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<bool> EditFormReporte(Ejecucion ejecucion)
        {
            try
            {

                Ejecucion original = await db.Ejecucion.FindAsync(ejecucion.id);
                    
                  
                //si no se tiene permisos por rol o por periodo para editar la programación, se deja la original
                if (!(User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1"))) || original.Periodo.EditarProgramacion != true)
                {
                    ejecucion.planeado = original.planeado;
                }
                //si no se tiene permisos por rol o por periodo para editar la ejecución, se deja la original
                if (!(User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1"))) || original.Periodo.EditarEjecucion != true)
                {
                  
                    ejecucion.ejecutado = original.ejecutado;
                }

                ejecucion.adjunto = original.adjunto;
                ejecucion.cargado = false;
                ejecucion.idindicador = original.idindicador;
                ejecucion.idperiodo = original.idperiodo;
                ejecucion.Nota = original.Nota;
                ejecucion.Periodo = original.Periodo;
                ejecucion.Indicador = original.Indicador;

                // Detach the Comparison State:
                db.Entry(original).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                return false;
            }



            if (ModelState.IsValid)
            {
                db.Entry(ejecucion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
