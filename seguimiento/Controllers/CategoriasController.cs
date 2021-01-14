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
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoriasController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


        [Authorize(Policy = "Configuracion.Responsable")]
        public async Task<ActionResult> GetFromResponsable(int idResponsable)
        {


            var categorias =await  db.Categoria.Where(n => n.IdResponsable == idResponsable).OrderBy(n => n.numero).ToListAsync();

            return View(categorias);
        }


        public async Task<ActionResult> Resumen(int ID)
        {
            EjecucionCategoriaController controladorEjecuciones = new EjecucionCategoriaController(db,userManager);
            NotasController controlnotas = new NotasController(db, userManager);
            ConfiguracionsController controlconfigutacion = new ConfiguracionsController(db,userManager);
            Configuracion config = await controlconfigutacion.Get();
            EvaluacionsController controlEvaluacion = new EvaluacionsController(db);


        List<Object> respuesta = new List<object>();
            List<EjecucionCategoria> listaEjecuciones = new List<EjecucionCategoria>();


            var categoria =await getFromId(ID);  //retoma la categoria por id desde la base de datos
            //if (categoria == null) { return new HttpStatusCodeResult(404, "no se encuentran categoria"); }  //error generado si no se encuentra la categoria

            if (config.PonderacionTipo == "PonderacionAbsoluta")
            {
                PonderacionAbsoluta pond = new PonderacionAbsoluta(db,userManager);
                categoria.Ponderador =await pond.CategoriaPonderador(ID);
            }

            int notasAbiertas = await controlnotas.NumeroNotasEstadoCategoriaId(ID, "Abierta");
            int notasCerradas = await controlnotas.NumeroNotasEstadoCategoriaId(ID, "Cerrada");

            ViewBag.notasAbiertas = notasAbiertas;
            ViewBag.notasCerradas = notasCerradas;

            listaEjecuciones = await controladorEjecuciones.GetFromCategoria(categoria.id);
            //======================================== obtiene los semanoforos===============================
            List<Evaluacion> evaluaciones = await controlEvaluacion.Get(categoria.id, "Categotia");

            var semaforos = controlEvaluacion.SetEvaluacionCategoria(listaEjecuciones, evaluaciones);

            //======================================== obtiene los semanoforos===============================
            object[] CategoriaConejecuciones = { categoria, listaEjecuciones, semaforos };
            respuesta.Add(CategoriaConejecuciones);


            ViewBag.categorias = respuesta;

            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();
            var Campos = db.Campo.Where(m => m.NivelPadre.id == categoria.Nivel.id || m.TodaCategoria == true).ToList();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Valor = db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.CategoriaPadre.id == categoria.id).FirstOrDefault();
                campos.Add(cp);
            }
            ViewBag.campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(respuesta);
        }


        public async Task<List<Categoria>> getFromNivel(int nivelNumero)
        {
            NIvelsController controlnivel = new NIvelsController(db,userManager);
            Nivel nivel = await controlnivel.getFromNumero(nivelNumero);
            List<Categoria> categorias = await db.Categoria.Where(n=>n.idNivel == nivel.id).ToListAsync();
            return categorias;
        }


        public async Task<Categoria> getFromId(int idcategoria)
        {
            Categoria categorias = await db.Categoria.Where(n => n.id == idcategoria).FirstOrDefaultAsync();


            return categorias;
        }

        public async Task<List<Categoria>> GetFomNivelAndParent(int idPadre, int nivel)
        {
            List<Categoria> categorias = await db.Categoria.Where(n => n.id == idPadre).ToListAsync();

            while (categorias[0].Nivel.numero < nivel)
            {
                List<int> idcategorias = new List<int>();
                foreach (Categoria categx in categorias)
                {
                    idcategorias.Add(categx.id);
                }

                string texto = String.Join(", ", idcategorias.ToArray());
                categorias = await db.Categoria.Where(n => idcategorias.Contains(n.idCategoria)).ToListAsync();

                if (categorias.Count == 0)
                {
                    break;
                }
            }


            return categorias;
        }

        public async Task<Dictionary<int,int>> CategoriasMenores(int id)
        {
            //apuntadores genéricdos 
            int n;
            //lista que guarda ids a consultar
            List<int> ids = new List<int>();
            //determina el id del nivel máximo el padre de todo
            var nivelMaximo = 0;
            var nivelMaximoOk = await db.Nivel.OrderByDescending(n=>n.numero).FirstOrDefaultAsync();
            if(nivelMaximoOk != null)
            {
                nivelMaximo = nivelMaximoOk.id;
            }
            //selecciona categorias hija de la principal
           var categorias = await db.Categoria.Where(n=>n.idCategoria == id).Select(gr => new
            {
                gr.id,
                gr.idNivel
            }
                    ).ToDictionaryAsync(n => n.id, n => n.idNivel);
            //List <Matrizx2> categorias = db.Database.SqlQuery<Matrizx2>("SELECT idNivel, id FROM Categorias WHERE idCategoria = " + id).ToList();
            //ciclo qu eva buscando categorias hasta lleagr alas categorias mas pequeñas
            if (categorias.Count > 0)
            {
                
                while (categorias.First().Value != nivelMaximo)
                {
                    //cra una lista con las categorias de las cuales se van a buscar los hijos
                    n = 0;
                    ids.Clear();
                    foreach (var categoria in categorias)
                    {
                        ids.Add(categoria.Key);
                        n++;
                    }
                    // obtiene las categorias 
                    string texto = String.Join(", ", ids.ToArray()); ;
                    categorias = await db.Categoria.Where(n=>ids.Contains(n.idCategoria)).Select(gr => new
                    {
                        gr.idNivel,
                        gr.id
                    }
                    ).ToDictionaryAsync(n => n.id, n => n.idNivel);
                //    categorias = db.Database.SqlQuery<Matrizx2>("SELECT idNivel, id FROM Categorias WHERE idCategoria IN (" + texto + ")").ToList();
                }
            }
            //si la cagtegoria no tiene hijas, la devuelve a ella
            else
            {
                categorias = await db.Categoria.Where(n => n.id == id).Select(gr => new
                {
                    gr.idNivel,
                    gr.id
                }
                    ).ToDictionaryAsync(n => n.id, n => n.idNivel);

              //  categorias = db.Database.SqlQuery<Matrizx2>("SELECT idNivel, id FROM Categorias WHERE id = " + id).ToList();
            }
            //retorna las categorias
            return categorias;
        }


       
    }
}
