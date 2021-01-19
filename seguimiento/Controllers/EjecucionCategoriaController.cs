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

        public async Task<List<EjecucionCategoria>> GetFromCategoriaYSubtotal(int id, int idPeriodo)
        {
            PeriodosController controlPeriodo = new PeriodosController(db, userManager);

            Periodo subtotal = await controlPeriodo.GetFromId(idPeriodo);

            List<Periodo> periodos = await controlPeriodo.PeriodosFromSubtotal(subtotal);

            List<int> ids = new List<int>();

            int n = 0;

            foreach (Periodo perx in periodos)
            {
                ids.Add(perx.id);
                n++;
            }
            // obtiene las categorias 
            string texto = String.Join(", ", ids.ToArray()); ;
            List<EjecucionCategoria> ejecuciones = new List<EjecucionCategoria>();

            if (texto != null && texto != "")
            {
                ejecuciones = await db.EjecucionCategoria.Where(n => n.IdCategoria == id && ids.Contains(n.idperiodo)).ToListAsync();
             }


            return ejecuciones;
        }

        public async Task<List<EjecucionCategoria>> GetFromCategoriaYTotal(int id, int idPeriodo)
        {
            PeriodosController controlPeriodo = new PeriodosController(db, userManager);
            Periodo total = await controlPeriodo.GetFromId(idPeriodo);

            List<Periodo> periodos = await controlPeriodo.PeriodosFromTotal(total);

            List<int> ids = new List<int>();

            int n = 0;

            foreach (Periodo perx in periodos)
            {
                ids.Add(perx.id);
                n++;
            }
            // obtiene las categorias 
            string texto = String.Join(", ", ids.ToArray()); ;
            List<EjecucionCategoria> ejecuciones = new List<EjecucionCategoria>();

            if (texto != null && texto != "")
            {
                ejecuciones = await db.EjecucionCategoria.Where(n => n.IdCategoria == id && ids.Contains(n.idperiodo)).ToListAsync();
                    
                  
            }


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

        public async Task<List<EjecucionCategoria>> GetHijosFromCatIDPerID(int idCat, int idPeriodo)
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);

            var categorias = await controlCategoria.getFromCategoria(idCat);

            List<EjecucionCategoria> respuesta = await GetFromCategorias(categorias, idPeriodo);

            return respuesta;
        }


        public async Task<List<EjecucionCategoria>> GetFromCategorias(List<Categoria> categorias, int idPeriodo)
        {
            List<int> ids = new List<int>();

            ids.Clear();
            foreach (Categoria categoria in categorias)
            {
                ids.Add(categoria.id);
            }
            // obtiene las categorias 
            string texto = String.Join(", ", ids.ToArray()); ;

            if (texto == "")
            {
                texto = "0";
            }

            List<EjecucionCategoria> ejecuciones =await  db.EjecucionCategoria.Where(n => ids.Contains(n.IdCategoria) && n.idperiodo == idPeriodo).ToListAsync();
                
                
                

            return ejecuciones;
        }

    }
}
