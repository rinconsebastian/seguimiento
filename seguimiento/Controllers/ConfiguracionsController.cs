using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
        public class ConfiguracionsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        



        public ConfiguracionsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        

        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Index2()
        {
            PropertyInfo[] propertyInfo = typeof(Configuracion).GetProperties();
            ViewBag.Props = propertyInfo;
            return View(await db.Configuracion.FirstAsync());
        }
       

        [Authorize(Policy = "Configuracion.General")]
        public async  Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Configuracion configuracion = await  db.Configuracion.FindAsync(id);
            if (configuracion == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(configuracion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Configuracion.General")]
        public async Task<IActionResult> Edit(Configuracion configuracion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index2");
            }
            return View(configuracion);
        }

        public async Task<bool> PermisoEditarEjecucion(System.Security.Claims.ClaimsPrincipal user, Ejecucion ejecucion)
        {
             ResponsablesController controlResponsable = new ResponsablesController(db, userManager);


            //---------------------- hernecia de responsabilidades

            var userFull = await userManager.FindByEmailAsync(user.Identity.Name);

            var ids = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);
                        
            if (((ids.Contains(ejecucion.Indicador.Categoria.IdResponsable)) &&
                user.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1") ||
                                        (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeado.Editar" && c.Value == "1")) &&
                                        (ejecucion.Periodo.EditarEjecucion == true))) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> PermisoMostrarEditarEjecucion(System.Security.Claims.ClaimsPrincipal user, Ejecucion ejecucion)
        {
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);

            var userFull = await userManager.FindByEmailAsync(user.Identity.Name);

            var ids = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);

            if (((ids.Contains(ejecucion.Indicador.Categoria.IdResponsable)) &&
                 user.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && c.Value == "1") ||
                                        (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1"))
                                        && (ejecucion.Periodo.EditarEjecucion == true || ejecucion.Periodo.EditarProgramacion == true))) 
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public async Task<bool> PermisoEditarEjecucionPlaneado(System.Security.Claims.ClaimsPrincipal user, Ejecucion ejecucion)
        {
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);

            var userFull = await userManager.FindByEmailAsync(user.Identity.Name);

            var ids = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia); //---------------------- hernecia de responsabilidades

            if ( ((ids.Contains(ejecucion.Indicador.Categoria.IdResponsable)) &&
                                        user.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && c.Value == "1")
                                        && (ejecucion.Periodo.EditarProgramacion == true)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> PermisoMostrarEditarIndicador(System.Security.Claims.ClaimsPrincipal user, Indicador indicador)
        {
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);
            var userFull = await userManager.FindByEmailAsync(user.Identity.Name);
            var ids = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia); //---------------------- hernecia de responsabilidades


            if (((ids.Contains(indicador.Categoria.IdResponsable)) && user.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar" && c.Value == "1")))
            //if (super || (usuario.IDDependencia == indicador.Categoria.IdResponsable && permiso ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Configuracion> Get()
        {
            Configuracion configuracion = await db.Configuracion.FirstOrDefaultAsync();

            return configuracion;
        }

        public string SqlErrorHandler(Exception exception)
        {

            string mensaje = "";
            DbUpdateConcurrencyException concurrencyEx = exception as DbUpdateConcurrencyException;
            if (concurrencyEx != null)
            {
                mensaje = "erro no identificado";
            }

            DbUpdateException dbUpdateEx = exception as DbUpdateException;
            if (dbUpdateEx != null)
            {
                if (dbUpdateEx.InnerException != null
                        && dbUpdateEx.InnerException.InnerException != null)
                {
                    SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                    if (sqlException != null)
                    {
                        switch (sqlException.Number)
                        {
                            case 2627:  // Unique constraint error
                                mensaje = "Ya existe un elemento con el mismo identificador unico";
                                break;
                            case 547:   // Constraint check violation
                                mensaje = "No se puede eliminar este item por que tiene elementos que dependen de el";
                                break;
                            case 2601:  // Duplicated key row error
                                mensaje = "Ya existe un elemento con el mismo identificador unico";
                                break;

                            default:
                                // A custom exception of yours for other DB issues
                                mensaje = "erro en la base de datos";
                                break;
                        }
                    }else
                    {
                        mensaje = dbUpdateEx.InnerException.ToString();
                    }


                }
            }

            return mensaje;
        }

        public bool Editable(int idresponsable, Categoria categoria, bool permiso, bool super)
        {
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);

            var ids = controlResponsable.GetAllIdsFromResponsable(idresponsable);

            //if ((responsable.Id == categoria.Responsable.Id && permiso) ||super)

            if (((ids.Contains(idresponsable)) && permiso) || super)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        // metodo para determinar si un usuario puede ver las notas dentro de una categoria
        public bool Visible(Configuracion configuracion, bool permiso, bool super)
        {
            if (configuracion.libre == true || permiso || super)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
