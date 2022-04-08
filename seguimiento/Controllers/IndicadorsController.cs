using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using seguimiento.Data;
using seguimiento.Formulas;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class IndicadorsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public IndicadorsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }


        public async Task<IActionResult> Listado(int itemId)
        {

            EvaluacionsController ControlEvaluacion = new EvaluacionsController(db);


            //--- variables
            List<Indicador> indicadores = new List<Indicador>(); // variable que almacena los indicadores devueltos por la base de datos
            List<object> listado = new List<object>();  //variable con toda la informacion a enviar a la vista
            UnidadesdeMedida Unidad = new UnidadesdeMedida();

            //---------recepcion de parametros
            var idPadre = itemId; //recibe el id padre desde la url


            //-----------consulta los indicadores que pertenecen a esa categoria si no lo logra genera alerta
            indicadores = await db.Indicador.Where(n => n.idCategoria == idPadre).Include(n => n.TipoIndicador).ToListAsync();



            foreach (var indicador in indicadores) // ciclo que se repite para cada indicador encontrado
            {
                //variables para cada indicador
                var ejecuciones = await db.Ejecucion.Where(n => n.idindicador == indicador.id).OrderBy(n => n.Periodo.orden).ToListAsync();
                //var ejecuciones = db.ejecucions.SqlQuery("select * from ejecucions where idindicador = " + indicador.id).ToList();  //Recupera de la base de datos el listado de ejecuciones existentes
                List<object> listadoEjecuciones = new List<object>(); //variable con las ejecuciones
                List<object> listadoParaSubtotal = new List<object>(); //variable con las ejecuciones necesarias para calcular cada subtotal
                List<object> listadoParaTotal = new List<object>(); //variable con los subtotales necesarios para calcular el total
                EjecucionCalculada respuesta = new EjecucionCalculada();
                decimal lineaBase = 0;
                string msg = "";

                //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
                ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                Object op = manejador.Unwrap();
                Type t = op.GetType();
                MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
                MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
                MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total


                //------------------ se definen las evaluaciones a aplicar para cada indicador
                List<Evaluacion> evaluaciones = await ControlEvaluacion.Get(indicador.id, "Indicador");

                foreach (var registro in ejecuciones)
                {
                    switch (registro.Periodo.tipo)
                    {
                        case "periodo":
                            object[] args = { registro, lineaBase }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, args); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(ControlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaSubtotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            break;
                        case "subtotal":
                            object[] argsSubtotal = { registro, listadoParaSubtotal, lineaBase }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorSubtotal.Invoke(op, argsSubtotal); //envia los argumentos mediante invoke al metodo Calculo_subtotal
                            listadoEjecuciones.Add(ControlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaTotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            listadoParaSubtotal.Clear();
                            break;
                        case "total":
                            object[] argstotal = { registro, listadoParaTotal }; //carga los argumentos en un objeto
                            respuesta = (EjecucionCalculada)operadorTotal.Invoke(op, argstotal); //envia los argumentos mediante invoke al metodo Calculo_total
                            listadoEjecuciones.Add(ControlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaTotal.Clear();
                            break;

                        case "lineabase":

                            var lb = registro.ejecutado;
                            if (lb != null)
                            {
                                lb = lb.Replace("%", "");
                                lb = Regex.Replace(lb, "^-$", "");
                                lb = Regex.Replace(lb, "^_$", "");
                                lb = Regex.Replace(lb, "[a-zA-Z^&()<>//:@#$%;+_!¡]", "");
                            }
                            try { lineaBase = string.IsNullOrEmpty(lb) ? 0 : System.Convert.ToDecimal(lb); }
                            catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                            catch (System.FormatException) { msg = "el valor ejecutado genera desbordamiento"; }
                            catch (System.ArgumentNullException) { msg = "el valor ejecutado genera desbordamiento"; }



                            object[] argslineabase = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argslineabase); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(Unidad.Formato(respuesta)); //almacena cada ejecucionCalcuada en la lista

                            break;
                        default:

                            object[] argsotros = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argsotros); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(Unidad.Formato(respuesta)); //almacena cada ejecucionCalcuada en la lista

                            break;

                    }
                }

                object[] indicadorConejecuciones = { indicador, listadoEjecuciones }; //objeto que unifica los datos del inidcaodr y sus ejecuciones
                listado.Add(indicadorConejecuciones);  //almacena el indicador y las ejecuciuones en el listado a enviar a la vista
            }
            ViewBag.listado = listado;
            return View();
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> ListadoUsuario()
        {

            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);
            EvaluacionsController controlEvaluacion = new EvaluacionsController(db);

            //--- variables
            List<Categoria> categorias = new List<Categoria>(); // variable que almacena los indicadores devueltos por la base de datos
            List<Indicador> indicadores = new List<Indicador>(); // variable que almacena los indicadores devueltos por la base de datos
            List<object> listado = new List<object>();  //variable con toda la informacion a enviar a la vista
            UnidadesdeMedida Unidad = new UnidadesdeMedida();



            //-------------------- VERIFICA PERMISOS DE ACUERDO AL ROL DE USUARIO --------------------------------
            // obtiene las categorias

            var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

            var idsx = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);

            //los vuleve un string
            string textox = String.Join(", ", idsx.ToArray()); ;

            //consulta las categorias sobre las que la dependencia del usuario tiene responsabilidad
            if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1")))
            {
                categorias = await db.Categoria.OrderBy(n => n.numero).ToListAsync();

            }
            else
            {
                categorias = await db.Categoria.Where(n => idsx.Contains(n.IdResponsable)).OrderBy(n => n.numero).ToListAsync();
            }

            List<int> ids = new List<int>();


            foreach (Categoria categoria in categorias)
            {
                ids.Add(categoria.id);
            }
            // obtiene las categorias 
            string texto = String.Join(", ", ids.ToArray()); ;

            //-----------consulta los indicadores que pertenecen a las categorias si no lo logra genera alerta
            //try { indicadores = db.Indicadors.SqlQuery("select i.* from Indicadors AS i, Categorias AS c where i.idCategoria =c.id AND c.id in ( " + texto + ")  Order By c.numero, i.codigo ,i.id").ToList(); }
            //catch { return new HttpStatusCodeResult(404, "no se han podido recuperar indicadores de la base de datos"); }

            indicadores = await db.Indicador.Where(n => ids.Contains(n.idCategoria)).OrderBy(n => n.Categoria.numero).OrderBy(n => n.codigo).OrderBy(n => n.id).ToListAsync();


            foreach (var indicador in indicadores) // ciclo que se repite para cada indicador encontrado
            {
                //variables para cada indicador
                var ejecuciones = await db.Ejecucion.Where(n => n.idindicador == indicador.id).OrderBy(n => n.Periodo.orden).ToListAsync();

                //var ejecuciones = db.ejecucions.SqlQuery("select * from ejecucions where idindicador = " + indicador.id).ToList();  //Recupera de la base de datos el listado de ejecuciones existentes
                List<object> listadoEjecuciones = new List<object>(); //variable con las ejecuciones
                List<object> listadoParaSubtotal = new List<object>(); //variable con las ejecuciones necesarias para calcular cada subtotal
                List<object> listadoParaTotal = new List<object>(); //variable con los subtotales necesarios para calcular el total
                EjecucionCalculada respuesta = new EjecucionCalculada();
                decimal lineaBase = 0;
                string msg = "";

                //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
                ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                Object op = manejador.Unwrap();
                Type t = op.GetType();
                MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
                MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
                MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total



                List<Evaluacion> evaluaciones = await controlEvaluacion.Get(indicador.id, "Indicador");
                foreach (var registro in ejecuciones)
                {
                    switch (registro.Periodo.tipo)
                    {
                        case "periodo":
                            object[] args = { registro, lineaBase }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, args); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(controlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaSubtotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            break;
                        case "subtotal":
                            object[] argsSubtotal = { registro, listadoParaSubtotal, lineaBase }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorSubtotal.Invoke(op, argsSubtotal); //envia los argumentos mediante invoke al metodo Calculo_subtotal
                            listadoEjecuciones.Add(controlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaTotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            listadoParaSubtotal.Clear();
                            break;
                        case "total":
                            object[] argstotal = { registro, listadoParaTotal }; //carga los argumentos en un objeto
                            respuesta = (EjecucionCalculada)operadorTotal.Invoke(op, argstotal); //envia los argumentos mediante invoke al metodo Calculo_total
                            listadoEjecuciones.Add(controlEvaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaTotal.Clear();
                            break;

                        case "lineabase":

                            var lb = registro.ejecutado;
                            if (lb != null)
                            {
                                lb = lb.Replace("%", "");
                                lb = Regex.Replace(lb, "^-$", "");
                                lb = Regex.Replace(lb, "^_$", "");
                                lb = Regex.Replace(lb, "[a-zA-Z^&()<>//:@#$%;+_!¡]", "");
                            }
                            try { lineaBase = string.IsNullOrEmpty(lb) ? 0 : System.Convert.ToDecimal(lb); }
                            catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                            catch (System.FormatException) { msg = "el valor ejecutado genera desbordamiento"; }
                            catch (System.ArgumentNullException) { msg = "el valor ejecutado genera desbordamiento"; }



                            object[] argslineabase = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argslineabase); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(Unidad.Formato(respuesta)); //almacena cada ejecucionCalcuada en la lista

                            break;
                        default:

                            object[] argsotros = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argsotros); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(Unidad.Formato(respuesta)); //almacena cada ejecucionCalcuada en la lista

                            break;

                    }
                }

                object[] indicadorConejecuciones = { indicador, listadoEjecuciones }; //objeto que unifica los datos del inidcaodr y sus ejecuciones
                listado.Add(indicadorConejecuciones);  //almacena el indicador y las ejecuciuones en el listado a enviar a la vista
            }
            ViewBag.listado = listado;
            return View();
        }

        public async Task<ActionResult> DetailsPop(int id, string tipo = "", string mensaje = "")
        {
            ConfiguracionsController configuracionControl = new ConfiguracionsController(db, userManager);
            Indicador indicador = new Indicador();

            var indicadorn = await db.Indicador.FindAsync(id);
            if (indicadorn != null)
            {
                indicador = indicadorn;

                //-------------------------------------------------------identificar si un usuario tiene acceso a editar una ejecucion

                if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar" && c.Value == "1")))
                {
                    ViewBag.MostrarBotonEditarIndicador = await configuracionControl.PermisoMostrarEditarIndicador(User, indicador);
                }
                else
                {
                    ViewBag.MostrarBotonEditarIndicador = false;
                }
                //-------------------------------------------------------identificar si un usuario tiene acceso a editar una ejecucion

                //-----------------------------Campos adicionales Inicio
                List<CampoValor> campos = new List<CampoValor>();
                var Campos = db.Campo.Where(m => m.TipoIndicadorPadre.Id == indicador.TipoIndicador.Id || m.TodoIndicador == true).ToList();
                foreach (Campo campon in Campos)
                {
                    CampoValor cp = new CampoValor();
                    cp.Campo = campon;
                    cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.IndicadorPadre.id == indicador.id).FirstOrDefaultAsync();
                    campos.Add(cp);
                }
                ViewBag.campos = campos;
                ViewBag.tipo = tipo;
                ViewBag.mensaje = mensaje;
                //-----------------------------Campos adicionales Fin
            }
            return View(indicador);
        }

        [HttpGet]
        public async Task<ActionResult> ChartPop(int id, string tipo)
        {
            List<IndicadorChartViewModel> dataset = new List<IndicadorChartViewModel>();

            List<EjecucionGrafica> ejecuciones = new List<EjecucionGrafica>();

            var periodos = await db.Periodo.Where(n => n.Ocultar == false && n.tipo == tipo).OrderBy(n => n.orden).Select(n => n.id).ToListAsync();

            if (tipo == "subtotal")
            {
                var ejecucionesN = await Ejecuciones(id);
                ejecuciones = ejecucionesN.Where(n=>n.Periodo.tipo == "subtotal")
                    .Select(n => new EjecucionGrafica
                    {
                        Periodo = n.Periodo.nombre,
                        Planeado = n.planeado,
                        Ejecutado = n.Periodo.cargado == true ? n.ejecutado:"",
                    }).ToList();
            }
            else
            {
                 ejecuciones = await db.Ejecucion.Where(n => n.idindicador == id && periodos.Contains(n.idperiodo)).OrderBy(n => n.Periodo.orden)
                   .Select(n => new EjecucionGrafica 
                   {
                       Periodo = n.Periodo.nombre,
                       Planeado = n.planeado,
                       Ejecutado = n.ejecutado
                   }).ToListAsync();
            }

            foreach (var eje in ejecuciones)
            {
                var item = new IndicadorChartViewModel();
                item.Periodo = eje.Periodo;
                item.Ejecutado = null;
                if (eje.Ejecutado != null && eje.Ejecutado != "")
                {
                    try { item.Ejecutado = System.Convert.ToDecimal(eje.Ejecutado); }
                    catch { }
                }
                item.Planeado = null;
                if (eje.Planeado != null && eje.Planeado != "")
                {
                    try { item.Planeado = System.Convert.ToDecimal(eje.Planeado); }
                    catch { }
                }
                dataset.Add(item);
            }
            return Json(dataset.ToArray());
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Editpop(int id)
        {
            Indicador indicador = await db.Indicador.FindAsync(id);

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = db.Categoria.ToList();
            foreach (var itemn in CategoriaListadb)
            { CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() }); }
            ViewBag.IdCategoria = new SelectList(CategoriaLista, "Value", "Text", indicador.Categoria.id);
            ViewBag.tipo = new SelectList(db.TipoIndicador, "Id", "Tipo", indicador.TipoIndicador.Id);



            List<SelectListItem> UnidadesLista = new List<SelectListItem>();
            UnidadesLista.Add(new SelectListItem() { Text = "Número", Value = "Numero" });
            UnidadesLista.Add(new SelectListItem() { Text = "Porcentaje", Value = "Porcentaje" });
            ViewBag.unidad = new SelectList(UnidadesLista, "Value", "Text", indicador.unidad);

            if (indicador == null)
            {
                //    return HttpNotFound();
            }

            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();

            var Campos = await db.Campo.Where(m => m.TipoIndicadorPadre.Id == indicador.TipoIndicador.Id || m.TodoIndicador == true).ToListAsync();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Campo.Nombre = cp.Campo.Nombre + "Add";
                cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.IndicadorPadre.id == indicador.id).FirstOrDefaultAsync();
                if (cp.Valor != null)
                {
                    cp.Valor.IndicadorPadre = null;
                }
                campos.Add(cp);
            }
            //Session["Campos"] = campos;
            HttpContext.Session.SetComplex("Campos", campos);
            ViewBag.campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(indicador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<bool> EditPop(Indicador indicador)
        {
            LogsController Logg = new LogsController(db, userManager);


            var campos = HttpContext.Session.GetComplex<List<CampoValor>>("Campos");

            if (indicador.codigo == "")
            {
                indicador.codigo = indicador.Categoria.numero;
            }
            if (ModelState.IsValid)
            {
                // Logg.CambioModelo("Indicador", "Edit", indicador); //registro log
                db.Entry(indicador).State = EntityState.Modified;
                await db.SaveChangesAsync();


                //------------------------------------------------- inicio almacenar campos adicionales
                //List<CampoValor> campos = (List<CampoValor>)Session["Campos"];
                foreach (CampoValor campon in campos)
                {
                    var valor = HttpContext.Request.Form[campon.Campo.Nombre].ToString();

                    if (campon.Valor != null)
                    {
                        ValorCampo valoredit = db.ValorCampo.Find(campon.Valor.Id);
                        valoredit.Texto = valor;
                        // db.Entry(original).State = EntityState.Detached;
                        //Logg.CambioModelo("CampoIndicador", "Edit", valoredit);
                        db.Entry(valoredit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ValorCampo valoradd = new ValorCampo();
                        valoradd.IdCampo = campon.Campo.Id;
                        valoradd.IndicadorPadre = indicador;
                        valoradd.Texto = valor;
                        db.ValorCampo.Add(valoradd);
                        db.SaveChanges();
                    }
                }


                HttpContext.Session.Remove("Campos");
                //------------------------------------------------- fin almacenar campos adicionales

                return true;
            }



            return false;
        }

        public async Task<List<Indicador>> getFromCategoria(int idcategorias)
        {
            List<Indicador> indicadores = await db.Indicador.Where(n => n.idCategoria == idcategorias).ToListAsync();


            return indicadores;
        }

        public async Task<List<Ejecucion>> getFromIndicador(int idIndicador)
        {
            List<Ejecucion> ejecuciones = await db.Ejecucion.Where(n => n.idindicador == idIndicador).ToListAsync();

            return ejecuciones;
        }

        public async Task<List<Indicador>> getFromCategorias(List<Categoria> categorias)
        {
            List<int> idcategorias = new List<int>();
            foreach (Categoria categx in categorias)
            {
                idcategorias.Add(categx.id);
            }

            //string texto = String.Join(", ", idcategorias.ToArray());
            // List<Indicador> indicadores = db.Indicadors.SqlQuery("select * from Indicadors where idCategoria IN (" + texto + ")").ToList();
            List<Indicador> indicadores = await db.Indicador.Where(N => idcategorias.Contains(N.idCategoria)).ToListAsync();


            return indicadores;
        }

        // GET: Indicadors
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Index()
        {
            string error = (string)HttpContext.Session.GetComplex<string>("error");
            if (error != "")
            {
                ViewBag.error = error;
                HttpContext.Session.Remove("error");
            }

            return View(await db.Indicador.OrderBy(n => n.Categoria.numero).ToListAsync());
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Create()
        {

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", "");
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", "");

            List<SelectListItem> UnidadesLista = new List<SelectListItem>();
            UnidadesLista.Add(new SelectListItem() { Text = "Número", Value = "Numero" });
            UnidadesLista.Add(new SelectListItem() { Text = "Porcentaje", Value = "Porcentaje" });
            ViewBag.unidad = new SelectList(UnidadesLista, "Value", "Text", "");


            return View();
        }

        // POST: Indicadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Create(Indicador indicador)
        {

            if ((indicador.codigo == null || indicador.codigo == "") && indicador.Categoria != null)
            {
                indicador.codigo = indicador.Categoria.numero;
            }

            EjecucionsController controlejecuciones = new EjecucionsController(db, userManager);
            if (ModelState.IsValid)
            {
                db.Indicador.Add(indicador);
                db.SaveChanges();
                bool resultado = await controlejecuciones.CrearEjecucionesDeIndicador(indicador);


                return RedirectToAction("Index");
            }

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            {
                CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() });
            }

            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", indicador.idCategoria);
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", indicador.tipo);

            List<SelectListItem> UnidadesLista = new List<SelectListItem>();
            UnidadesLista.Add(new SelectListItem() { Text = "Número", Value = "Numero" });
            UnidadesLista.Add(new SelectListItem() { Text = "Porcentaje", Value = "Porcentaje" });
            ViewBag.unidad = new SelectList(UnidadesLista, "Value", "Text", indicador.unidad);

            return View(indicador);
        }


        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Edit(int id)
        {

            Indicador indicador = await db.Indicador.FindAsync(id);
            if (indicador == null) { return NotFound(); }

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            { CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() }); }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", indicador.Categoria.id);
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", indicador.TipoIndicador.Id);

            List<SelectListItem> UnidadesLista = new List<SelectListItem>();
            UnidadesLista.Add(new SelectListItem() { Text = "Número", Value = "Numero" });
            UnidadesLista.Add(new SelectListItem() { Text = "Porcentaje", Value = "Porcentaje" });
            ViewBag.unidad = new SelectList(UnidadesLista, "Value", "Text", indicador.unidad);


            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();

            var Campos = await db.Campo.Where(m => m.TipoIndicadorPadre.Id == indicador.TipoIndicador.Id || m.TodoIndicador == true).ToListAsync();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Campo.Nombre = cp.Campo.Nombre + "Add";
                cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.IndicadorPadre.id == indicador.id).FirstOrDefaultAsync();
                if (cp.Valor != null)
                {
                    cp.Valor.IndicadorPadre = null;
                }
                campos.Add(cp);
            }
            HttpContext.Session.SetComplex("Campos", campos);
            ViewBag.Campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(indicador);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Edit(Indicador indicador)
        {

            if ((indicador.codigo == null || indicador.codigo == "") && indicador.Categoria != null)
            {
                indicador.codigo = indicador.Categoria.numero;
            }

            if (ModelState.IsValid)
            {
                db.Entry(indicador).State = EntityState.Modified;
                await db.SaveChangesAsync();

                //------------------------------------------------- inicio almacenar campos adicionales
                var campos = HttpContext.Session.GetComplex<List<CampoValor>>("Campos");

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
                        valoradd.IndicadorPadre = indicador;
                        valoradd.Texto = valor;
                        await db.ValorCampo.AddAsync(valoradd);
                        await db.SaveChangesAsync();
                    }
                }
                HttpContext.Session.Remove("Campos");
                //------------------------------------------------- fin almacenar campos adicionales

                return RedirectToAction("Index");
            }

            List<SelectListItem> CategoriaLista = new List<SelectListItem>();
            var CategoriaListadb = await db.Categoria.ToListAsync();
            foreach (var itemn in CategoriaListadb)
            { CategoriaLista.Add(new SelectListItem() { Text = itemn.numero + " " + itemn.nombre, Value = itemn.id.ToString() }); }
            ViewBag.Categorias = new SelectList(CategoriaLista, "Value", "Text", indicador.idCategoria);
            ViewBag.Tipos = new SelectList(await db.TipoIndicador.ToListAsync(), "Id", "Tipo", indicador.tipo);

            List<SelectListItem> UnidadesLista = new List<SelectListItem>();
            UnidadesLista.Add(new SelectListItem() { Text = "Número", Value = "Numero" });
            UnidadesLista.Add(new SelectListItem() { Text = "Porcentaje", Value = "Porcentaje" });
            ViewBag.unidad = new SelectList(UnidadesLista, "Value", "Text", indicador.unidad);


            //regenerar campos adicionales en viewbag
            ViewBag.Campos = HttpContext.Session.GetComplex<List<CampoValor>>("Campos");




            return View(indicador);
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Delete(int id)
        {//-------------------- VERIFICA PERMISOS DE ACUERDO AL ROL DE USUSARIO --------------------------------

            Indicador indicador = await db.Indicador.FindAsync(id);
            if (indicador == null) { return NotFound(); }

            return View(indicador);
        }

        // POST: Indicadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            string error = "";
            ConfiguracionsController controlConfiguracion = new ConfiguracionsController(db, userManager);
            EjecucionsController controlEjecucion = new EjecucionsController(db, userManager);
            CamposController controlCampos = new CamposController(db, userManager);

            Indicador indicador = await db.Indicador.FindAsync(id);
            var resultadoEjeccuines = await controlEjecucion.BorrarEjecucionesDeIndicador(id);
            if (resultadoEjeccuines == false)
            {
                HttpContext.Session.SetComplex("error", "Imposible eliminar las ejecuciones del indicador");
            }
            else
            {
                var resultadoCampos = await controlCampos.BorrarDeIndicador(id);
                if (resultadoCampos == false)
                {
                    HttpContext.Session.SetComplex("error", "Imposible eliminar las valores adicionales");
                }
                else
                {

                    try
                    {
                        db.Indicador.Remove(indicador);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        error = controlConfiguracion.SqlErrorHandler(ex);
                        HttpContext.Session.SetComplex("error", error);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<List<Indicador>> GetAll()
        {
            List<Indicador> respuesta = await db.Indicador.ToListAsync();
            return respuesta;
        }


        // GET: Indicadors/Details/5
        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Details(int? id)
        {

            Indicador indicador = await db.Indicador.FindAsync(id);
            if (indicador == null) { return NotFound(); }

            //-----------------------------Campos adicionales Inicio
            List<CampoValor> campos = new List<CampoValor>();
            var Campos = await db.Campo.Where(m => m.TipoIndicadorPadre.Id == indicador.TipoIndicador.Id || m.TodoIndicador == true).ToListAsync();
            foreach (Campo campon in Campos)
            {
                CampoValor cp = new CampoValor();
                cp.Campo = campon;
                cp.Valor = await db.ValorCampo.Where(m => m.CampoPadre.Id == campon.Id && m.IndicadorPadre.id == indicador.id).FirstOrDefaultAsync();
                campos.Add(cp);
            }
            ViewBag.Campos = campos;
            //-----------------------------Campos adicionales Fin

            return View(indicador);
        }

        [Authorize(Policy = "Indicador.Editar")]
        public async Task<ActionResult> Reportar()
        {
            ResponsablesController controlResponsable = new ResponsablesController(db, userManager);

            List<Indicador> indicadores = new List<Indicador>();

            List<ReporteViewModel> reporte = new List<ReporteViewModel>();

            if (User.HasClaim(c => (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Super" && c.Value == "1")))
            {
                indicadores = await db.Indicador.OrderBy(n => n.Categoria.numero).ThenBy(n => n.codigo).ThenBy(n => n.id).ToListAsync();

            }
            else
            {
                var userFull = await userManager.FindByEmailAsync(User.Identity.Name);

                var ids = controlResponsable.GetAllIdsFromResponsable(userFull.IDDependencia);

                indicadores = await db.Indicador.Where(n => ids.Contains(n.Categoria.IdResponsable))
                    .Include(n => n.Categoria)
                    .Include(n => n.Categoria.CategoriaPadre)
                    .Include(n => n.Categoria.Nivel)
                    .Include(n => n.TipoIndicador)
                    .OrderBy(n => n.Categoria.numero).ThenBy(n => n.codigo).ThenBy(n => n.id).AsNoTracking().ToListAsync();

                //indicadores = db.Indicadors.Where(n => n.Categoria.IdResponsable ).OrderBy(n=>n.Categoria.numero).ThenBy(n => n.id).ToList();
            }
            foreach (var Indicador in indicadores)
            {
                Indicador.Categoria.Responsable = null;
                if (Indicador.Categoria.CategoriaPadre != null) { Indicador.Categoria.CategoriaPadre = null; }


                ReporteViewModel item = new ReporteViewModel();
                item.Indicador = Indicador;
                var ejecuciones = await db.Ejecucion.Where(n => n.idindicador == Indicador.id && (n.Periodo.EditarEjecucion == true || n.Periodo.EditarProgramacion == true)).OrderBy(n => n.Periodo.orden).ToListAsync();

                foreach (var ejecucion in ejecuciones)
                {
                    ejecucion.Indicador.Categoria.Responsable = null;
                    if (ejecucion.Indicador.Categoria.CategoriaPadre != null) { ejecucion.Indicador.Categoria.CategoriaPadre = null; }
                }
                //var ejecuciones = db.ejecucions.SqlQuery("select * FROM ejecucions AS e, Periodoes AS p WHERE p.id=e.idperiodo AND e.idindicador="+Indicador.id+" AND (p.EditarEjecucion = 'TRUE' or P.EditarProgramacion= 'TRUE' )").ToList();

                item.Ejecuciones = ejecuciones;

                reporte.Add(item);
            }



            HttpContext.Session.SetComplex("reporte", reporte);


            return View(reporte);
        }

        public ActionResult ReportExportExcel()
        {
            var reporte = (List<ReporteViewModel>)HttpContext.Session.GetComplex<List<ReporteViewModel>>("reporte");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Indicador";

                foreach (var registro in reporte)
                {
                    if (currentRow == 1)
                    {
                        var currentColumn0 = 3;
                        foreach (var ejecucion in registro.Ejecuciones)
                        {
                            worksheet.Cell(currentRow, currentColumn0).Value = ejecucion.Periodo.nombre + "-Planeado";
                            currentColumn0++;
                            worksheet.Cell(currentRow, currentColumn0).Value = ejecucion.Periodo.nombre + "-Ejecutado";
                            currentColumn0++;
                        }
                    }

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = registro.Indicador.id;
                    worksheet.Cell(currentRow, 2).Value = registro.Indicador.nombre;

                    var currentColumn = 3;
                    foreach (var ejecucion in registro.Ejecuciones)
                    {
                        worksheet.Cell(currentRow, currentColumn).Value = ejecucion.planeado;
                        currentColumn++;
                        worksheet.Cell(currentRow, currentColumn).Value = ejecucion.ejecutado;
                        currentColumn++;

                    }
                }




                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }

        }

        private async Task<List<EjecucionCalculada>> Ejecuciones(int idIndicador)
        {
            var indicador = await db.Indicador.FindAsync(idIndicador);

            //variables para cada indicador
            var ejecuciones = await db.Ejecucion.Where(n => n.idindicador == indicador.id).OrderBy(n => n.Periodo.orden).ToListAsync();
            //var ejecuciones = db.ejecucions.SqlQuery("select * from ejecucions where idindicador = " + indicador.id).ToList();  //Recupera de la base de datos el listado de ejecuciones existentes
            List<EjecucionCalculada> listadoEjecuciones = new List<EjecucionCalculada>(); //variable con las ejecuciones
            List<object> listadoParaSubtotal = new List<object>(); //variable con las ejecuciones necesarias para calcular cada subtotal
            List<object> listadoParaTotal = new List<object>(); //variable con los subtotales necesarios para calcular el total
            EjecucionCalculada respuesta = new EjecucionCalculada();
            decimal lineaBase = 0;
            string msg = "";

            //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
            ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
            Object op = manejador.Unwrap();
            Type t = op.GetType();
            MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
            MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
            MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total




            foreach (var registro in ejecuciones)
            {
                switch (registro.Periodo.tipo)
                {
                    case "periodo":
                        object[] args = { registro, lineaBase }; //carga los argumentos en un objeto 
                        respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, args); //envia los argumentos mediante invoke al metodo Calculo_periodo
                        listadoEjecuciones.Add(respuesta); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                        listadoParaSubtotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                        break;
                    case "subtotal":
                        object[] argsSubtotal = { registro, listadoParaSubtotal, lineaBase }; //carga los argumentos en un objeto 
                        respuesta = (EjecucionCalculada)operadorSubtotal.Invoke(op, argsSubtotal); //envia los argumentos mediante invoke al metodo Calculo_subtotal
                        listadoEjecuciones.Add(respuesta); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                        listadoParaTotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                        listadoParaSubtotal.Clear();
                        break;
                    case "total":
                        object[] argstotal = { registro, listadoParaTotal }; //carga los argumentos en un objeto
                        respuesta = (EjecucionCalculada)operadorTotal.Invoke(op, argstotal); //envia los argumentos mediante invoke al metodo Calculo_total
                        listadoEjecuciones.Add(respuesta); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                        listadoParaTotal.Clear();
                        break;

                    case "lineabase":

                        var lb = registro.ejecutado;
                        if (lb != null)
                        {
                            lb = lb.Replace("%", "");
                            lb = Regex.Replace(lb, "^-$", "");
                            lb = Regex.Replace(lb, "^_$", "");
                            lb = Regex.Replace(lb, "[a-zA-Z^&()<>//:@#$%;+_!¡]", "");
                        }
                        try { lineaBase = string.IsNullOrEmpty(lb) ? 0 : System.Convert.ToDecimal(lb); }
                        catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                        catch (System.FormatException) { msg = "el valor ejecutado genera desbordamiento"; }
                        catch (System.ArgumentNullException) { msg = "el valor ejecutado genera desbordamiento"; }



                        object[] argslineabase = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                        respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argslineabase); //envia los argumentos mediante invoke al metodo Calculo_periodo
                        listadoEjecuciones.Add(respuesta); //almacena cada ejecucionCalcuada en la lista

                        break;
                    default:

                        object[] argsotros = { registro, (decimal)10.00 }; //carga los argumentos en un objeto 
                        respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, argsotros); //envia los argumentos mediante invoke al metodo Calculo_periodo
                        listadoEjecuciones.Add(respuesta); //almacena cada ejecucionCalcuada en la lista

                        break;

                }
            }


            return listadoEjecuciones;
        }

    }

}
