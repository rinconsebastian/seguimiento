using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class RolController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        private RoleManager<ApplicationRole> roleManager;

        public RolController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public RolController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> roleMgr)
        {
            db = context;
            userManager = _userManager;
            roleManager = roleMgr;
        }

        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Index()
        {
            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }
            var Roles = await db.Roles.ToListAsync();
            return View(Roles);
        }

        [Authorize(Policy = "Rol.Editar")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Create( ApplicationRole Rol)
        {
            var normalized = Rol.Name.ToUpper();
            var validation = await db.Roles.Where(n => n.NormalizedName.StartsWith(normalized)).CountAsync();
            Rol.NormalizedName = validation == 0 ? normalized : normalized + "_" + validation ;
            if (ModelState.IsValid)
            {
                
                db.Roles.Add(Rol);
                await db.SaveChangesAsync();

                var policies = await db.Policy.ToListAsync();
                var claims = new List<IdentityRoleClaim<string>>();
                foreach(var p in policies)
                {
                    var claim = new IdentityRoleClaim<string>();
                    claim.RoleId = Rol.Id;
                    claim.ClaimValue = "0";
                    claim.ClaimType = p.claim;
                    claims.Add(claim);

                }
                db.RoleClaims.AddRange(claims);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(Rol);
        }

        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Details(string id)
        {
            ApplicationRole Rol = await db.Roles.FindAsync(id);
            if (Rol == null){return NotFound();}

            var policies = await db.Policy.ToDictionaryAsync(n => n.claim, n => n.nombre);
            var permisos = await db.RoleClaims.Where(n => n.RoleId == Rol.Id)
                .Select(n => new PermisoDataModel
                {
                    id = n.Id,
                    valor = n.ClaimValue,
                    texto = n.ClaimType
                })
                .ToListAsync();

            foreach (var per in permisos)
            {
                if (policies.ContainsKey(per.texto))
                {
                    per.texto = policies[per.texto];
                }
            }
            ViewBag.Permisos = permisos;

            return View(Rol);
        }

        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole Rol = await db.Roles.FindAsync(id);
            if (Rol == null){return NotFound();}

            var policies = await db.Policy.ToDictionaryAsync(n => n.claim, n => n.nombre);
            var permisos = await db.RoleClaims.Where(n => n.RoleId == Rol.Id)
                .Select(n => new PermisoDataModel {
                    id = n.Id,
                    valor = n.ClaimValue,
                    texto = n.ClaimType
                })
                .ToListAsync();

            foreach(var per in permisos)
            {
                if (policies.ContainsKey(per.texto))
                {
                    per.texto = policies[per.texto];
                }
            }
            ViewBag.Permisos = permisos;

            return View(Rol);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Edit(ApplicationRole Rol)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Rol).State = EntityState.Modified;
                await db.SaveChangesAsync();

                // INICIO - Guarda permisos
                var permisosDb = await db.RoleClaims.Where(n => n.RoleId == Rol.Id).ToListAsync();
                foreach (var per in permisosDb)
                {
                    var valor = HttpContext.Request.Form["p_"+per.Id].ToString();
                    if (valor != null)
                    {
                        per.ClaimValue = valor == "" ? "0" : valor;
                        db.Entry(per).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                //Fin - Guarda permisos

                return RedirectToAction("Index");
            }


            var policies = await db.Policy.ToDictionaryAsync(n => n.claim, n => n.nombre);
            var permisos = await db.RoleClaims.Where(n => n.RoleId == Rol.Id)
                .Select(n => new PermisoDataModel
                {
                    id = n.Id,
                    valor = n.ClaimValue,
                    texto = n.ClaimType
                })
                .ToListAsync();

            foreach (var per in permisos)
            {
                if (policies.ContainsKey(per.texto))
                {
                    per.texto = policies[per.texto];
                }
            }
            ViewBag.Permisos = permisos;
            return View(Rol);
        }
        
        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationRole Rol = await db.Roles.FindAsync(id);
            if (Rol == null){return NotFound();}

            return View(Rol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Rol.Editar")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);

            ApplicationRole Rol = await db.Roles.FindAsync(id);
            try
            {
                var claims = await db.RoleClaims.Where(n => n.RoleId == Rol.Id).ToListAsync();
                db.RoleClaims.RemoveRange(claims);
                await db.SaveChangesAsync();

                db.Roles.Remove(Rol);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = controlConfiguracion.SqlErrorHandler(ex);
            }
            HttpContext.Session.SetComplex("error", error);

            return RedirectToAction("Index");
        }

    }
}
