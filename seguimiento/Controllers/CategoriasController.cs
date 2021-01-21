using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            NivelsController controlnivel = new NivelsController(db,userManager);
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

        public async Task<List<Categoria>> getFromCategoria(int idcategoria)
        {
            List<Categoria> categorias = await db.Categoria.Where(n => n.idCategoria == idcategoria).ToListAsync();
                
               


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



        // GET: Categorias
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Index()
        {



            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }

            // var categorias = db.Categorias.Include(c => c.CategoriaPadre).Include(c => c.Nivel);
            //var categorias = db.Categorias;
            var categorias =await db.Categoria.Include(c => c.Nivel).OrderBy(n => n.numero).ToListAsync();
            return View(categorias);
        }

        // GET: Categorias/Details/5
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Details(int id)
        {
           
          
            Categoria categoria = await db.Categoria.FindAsync(id);
           
            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();
            var Campos =await db.Campo.Where(m => m.NivelPadre.id == categoria.Nivel.id || m.TodaCategoria == true).ToListAsync();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.CategoriaPadre.id == categoria.id).FirstOrDefaultAsync();
                campos.Add(cp);
            }
            ViewBag.campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(categoria);
        }

        // GET: Categorias/Create
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Create()
        {
            
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            { CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() }); }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", "");
            ViewBag.Responsables = new SelectList(await db.Responsable.ToListAsync(), "Id", "Nombre", "");
            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", "");
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Create( Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await db.Categoria.AddAsync(categoria);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb =await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", categoria.idCategoria);
            ViewBag.Responsables = new SelectList(await db.Responsable.ToListAsync(), "Id", "Nombre", categoria.IdResponsable);
            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", categoria.idNivel);
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Edit(int id)
        {
                      
            Categoria categoria = await db.Categoria.FindAsync(id);
            
            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.Where(n => n.id != id).ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", categoria.idCategoria);
            ViewBag.Responsables = new SelectList(await db.Responsable.ToListAsync(), "Id", "Nombre", categoria.IdResponsable);
            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", categoria.idNivel);

            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();

            var Campos = await db.Campo.Where(m => m.NivelPadre.id == categoria.Nivel.id || m.TodaCategoria == true).ToListAsync();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Campo.Nombre = cp.Campo.Nombre + "Add";
                cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.CategoriaPadre.id == categoria.id).FirstOrDefaultAsync();
                if (cp.Valor != null)
                {
                    cp.Valor.CategoriaPadre = null;
                }
                campos.Add(cp);
            }
            
            HttpContext.Session.SetComplex("Campos", campos);
            ViewBag.Campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Edit( Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                await db.SaveChangesAsync();

                // INICIO - Guarda los campos adicionales
                List<CampoValor> campos = HttpContext.Session.GetComplex<List<CampoValor>>("Campos");
                foreach (CampoValor campon in campos)
                {
                    var valor = HttpContext.Request.Form[campon.Campo.Nombre].ToString();
                    if (campon.Valor != null)
                    {
                        ValorCampo valoredit = await db.ValorCampo.FindAsync(campon.Valor.Id);
                        valoredit.Texto = valor;
                        // db.Entry(original).State = EntityState.Detached;
                        db.Entry(valoredit).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        ValorCampo valoradd = new ValorCampo();
                        valoradd.IdCampo = campon.Campo.Id;
                        valoradd.CategoriaPadre = categoria;
                        valoradd.Texto = valor;
                        await db.ValorCampo.AddAsync(valoradd);
                        await db.SaveChangesAsync();
                    }
                }
                HttpContext.Session.Remove("Campos");
                // FIN - almacenar campos adicionales
                return RedirectToAction("Index");
            }

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.Where(n => n.id != categoria.id).ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", categoria.idCategoria);
            ViewBag.Responsables = new SelectList(await db.Responsable.ToListAsync(), "Id", "Nombre", categoria.IdResponsable);
            ViewBag.Niveles = new SelectList(await db.Nivel.ToListAsync(), "id", "nombre", categoria.idNivel);

            //regenerar campos adicionales en viewbag
            ViewBag.Campos = HttpContext.Session.GetComplex<List<CampoValor>>("Campos");

            return View(categoria);
        }

        // GET: Categorias/Delete/5
       [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> Delete(int id)
        {
            
            Categoria categoria = await db.Categoria.FindAsync(id);
            
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Categoria.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            
            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db,userManager);

            Categoria categoria = await db.Categoria.FindAsync(id);
            try
            {
                db.Categoria.Remove(categoria);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                error = controlConfiguracion.SqlErrorHandler(ex);
                HttpContext.Session.SetComplex("error", error);
            }


            return RedirectToAction("Index");
        }

        public async Task<Categoria> getMain()
        {
            var nivelmin = await db.Nivel.OrderBy(n => n.numero).FirstOrDefaultAsync();
            Categoria categoria = await db.Categoria.Where(n => n.idNivel == nivelmin.id).FirstOrDefaultAsync();
               


            return categoria;
        }

        public async Task<int> NumeroIndicadores(int idcategoria)
        {
            int respuesta = 0;


            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);

            List<Categoria> categorias = await CategoriasMenoresList(idcategoria);


            List<Indicador> indicadores = await controlIndicador.getFromCategorias(categorias);

            respuesta = indicadores.Where(n => n.ponderador > 0).Count();


            return respuesta;


        }

        public async Task<List<Categoria>> CategoriasMenoresList(int id)
        {
            //apuntadores genéricdos 
            int n;
            //lista que guarda ids a consultar
            List<int> ids = new List<int>();
            //determina el id del nivel máximo el padre de todo
            var numeroNivleMax = await db.Nivel.OrderByDescending(n => n.numero).FirstOrDefaultAsync();
            var nivelMaximo = numeroNivleMax.id;

            //selecciona categorias hija de la principal
            List<Categoria> categorias = await db.Categoria.Where(n => n.idCategoria == id).ToListAsync();
               
               //ciclo qu eva buscando categorias hasta lleagr alas categorias mas pequeñas
            if (categorias.Count > 0)
            {
                while (categorias[0].idNivel != nivelMaximo)
                {
                    //cra una lista con las categorias de las cuales se van a buscar los hijos
                    n = 0;
                    ids.Clear();
                    foreach (Categoria categoria in categorias)
                    {
                        ids.Add(categorias[n].id);
                        n++;
                    }
                    // obtiene las categorias 
                    string texto = String.Join(", ", ids.ToArray()); ;
                    categorias = await db.Categoria.Where(n => ids.Contains(n.idCategoria)).ToListAsync();
                     
                }
            }
            //si la cagtegoria no tiene hijas, la devuelve a ella
            else
            {
                categorias = await db.Categoria.Where(n => n.id == id).ToListAsync();
               
            }
            //retorna las categorias
            return categorias;
        }
    }
}
