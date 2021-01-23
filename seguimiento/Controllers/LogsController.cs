using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace seguimiento.Controllers
{
    public class LogsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

       

        public LogsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public void CambioModelo(string Modelo, string Accion, object ContenidoNew, string usuario)
        {
            
            if (1 == 1)
            {

                string newModelString = "";
                string oldModelString = "";

                switch (Modelo)
                {
                    case "Ejecucion":
                        Ejecucion newModel = (Ejecucion)ContenidoNew;
                        newModelString = new JavaScriptSerializer().Serialize(newModel);
                        if (newModel.id != 0)
                        {
                            Ejecucion oldModel = db.Ejecucion.Where(n=> n.id == newModel.id).FirstOrDefault();
                            oldModelString = new JavaScriptSerializer().Serialize(new { id = oldModel.id, idindicador = oldModel.idindicador, idperiodo = oldModel.idperiodo, planeado = oldModel.planeado, ejecutado = oldModel.ejecutado, cargado = oldModel.cargado, Nota = oldModel.Nota, adjunto = oldModel.adjunto });

                        }
                        break;

                    case "Indicador":
                        Indicador newModel2 = (Indicador)ContenidoNew;
                        newModelString = new JavaScriptSerializer().Serialize(newModel2);
                        if (newModel2.id != 0)
                        {
                            Indicador oldModel2 =  db.Indicador.Find(newModel2.id);
                            oldModelString = new JavaScriptSerializer().Serialize(new { id = oldModel2.id, idCategoria = oldModel2.idCategoria, ponderador = oldModel2.ponderador, tipo = oldModel2.tipo, TipoIndicador = oldModel2.TipoIndicador, codigo = oldModel2.codigo, unidad = oldModel2.unidad, nombre = oldModel2.nombre, nota = oldModel2.Nota, adjunto = oldModel2.adjunto });

                        }
                        break;
                    case "CampoIndicador":
                        ValorCampo newModel3 = (ValorCampo)ContenidoNew;
                        newModelString = new JavaScriptSerializer().Serialize(newModel3);
                        if (newModel3.Id != 0)
                        {
                            ValorCampo oldModel3 = db.ValorCampo.Find(newModel3.Id);
                            oldModelString = new JavaScriptSerializer().Serialize(new { Id = oldModel3.Id, IdCampo = oldModel3.IdCampo, CampoPadre = oldModel3.CampoPadre.Id, Texto = oldModel3.Texto });
                        }
                        break;


                }


                Log log = new Log();
                log.UserId = usuario;
                log.UserName = usuario;
                log.UserMail = usuario;
                log.Tarea = Modelo;
                log.Accion = Accion;
                log.ContenidoNew = newModelString;
                log.ContenidoOld = oldModelString;


                 db.Log.Add(log);
                 db.SaveChanges();
            }

        }


        [Authorize(Policy = "Configuracion.Logs")]
        public async Task<ActionResult> Index(string resultado)
        {
            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }
            var logs = await db.Log.OrderByDescending(n => n.TimeStamp).Take(100).ToListAsync();
            return View(logs);
        }

        [Authorize(Policy = "Configuracion.Logs")]
        public async Task<ActionResult> Details(int Id)
        {
            Log log = await db.Log.FindAsync(Id);
            if (log == null) { return NotFound(); }
            ViewBag.Old = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(log.ContenidoOld), Formatting.Indented);
            ViewBag.New = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(log.ContenidoNew), Formatting.Indented);
            return View(log);
        }
    }
}
