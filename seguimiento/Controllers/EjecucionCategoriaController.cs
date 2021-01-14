using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class EjecucionCategoriaController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        public EjecucionCategoriaController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        public void BorrarTodo()
        {
            if (ModelState.IsValid)
            {
                while ((db.EjecucionCategoria.Count()) > 0)
                {

                    db.Database.ExecuteSqlRaw("TRUNCATE `EjecucionCategoria`;");

                    db.SaveChanges();

                }
            }

        }

        public async Task<bool> Crear(EjecucionCategoria ejecucion)
        {
            if (ModelState.IsValid)
            {
                await db.EjecucionCategoria.AddAsync(ejecucion);
                await db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<EjecucionCategoria>> GetFromCategoria(int id)
        {


            //List<EjecucionCategoria> ejecuciones = db.EjecucionCategorias.SqlQuery("select * from EjecucionCategorias where IdCategoria = " + id).ToList();
            List<EjecucionCategoria> ejecuciones = await db.EjecucionCategoria.Where(n => n.IdCategoria == id).OrderBy(n => n.Periodo.orden).ToListAsync();

            return ejecuciones;
        }

        public async Task<bool> Evaluar()
        {
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);
            var configuracion = await controlConfiguracion.Get();

            if (configuracion.PonderacionTipo == "PonderacionRelativa")
            {
               //PonderacionRelativa ponderacion = new PonderacionRelativa();
               // var casa = ponderacion.Calculo_total_categoria(configuracion);
            }
            if (configuracion.PonderacionTipo == "PonderacionAbsoluta")
            {
                PonderacionAbsoluta ponderacion = new PonderacionAbsoluta(db,userManager);
                var casa =await ponderacion.Calculo_total_categoria(configuracion);
            }

            return true;
        }

    }
}
