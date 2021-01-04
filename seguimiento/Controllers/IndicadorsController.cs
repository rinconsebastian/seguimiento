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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace seguimiento.Controllers
{
    public class IndicadorsController : Controller
    {
       
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly EvaluacionsController evaluacion ;

        public IndicadorsController(ApplicationDbContext context, UserManager<ApplicationUser> _userManager, EvaluacionsController _evaluacion)
        {
            db = context;
            userManager = _userManager;
            evaluacion = _evaluacion;
        }

        
        public async Task<IActionResult> Listado(int itemId)
        {
         
          

            //--- variables
            List<Indicador> indicadores = new List<Indicador>(); // variable que almacena los indicadores devueltos por la base de datos
            List<object> listado = new List<object>();  //variable con toda la informacion a enviar a la vista
            UnidadesdeMedida Unidad = new UnidadesdeMedida();

            //---------recepcion de parametros
            var idPadre = itemId; //recibe el id padre desde la url


            //-----------consulta los indicadores que pertenecen a esa categoria si no lo logra genera alerta
            indicadores = await db.Indicador.Where(n => n.idCategoria == idPadre).Include(n=>n.TipoIndicador).ToListAsync();
           


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
                ObjectHandle manejador = Activator.CreateInstance(null, "Seguimiento0._1.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                Object op = manejador.Unwrap();
                Type t = op.GetType();
                MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
                MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
                MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total


                //------------------ se definen las evaluaciones a aplicar para cada indicador
                List<Evaluacion> evaluaciones = await evaluacion.Get(indicador.id, "Indicador");

                foreach (var registro in ejecuciones)
                {
                    switch (registro.Periodo.tipo)
                    {
                        case "periodo":
                            object[] args = { registro, lineaBase }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorPeriodo.Invoke(op, args); //envia los argumentos mediante invoke al metodo Calculo_periodo
                            listadoEjecuciones.Add(evaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaSubtotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            break;
                        case "subtotal":
                            object[] argsSubtotal = { registro, listadoParaSubtotal }; //carga los argumentos en un objeto 
                            respuesta = (EjecucionCalculada)operadorSubtotal.Invoke(op, argsSubtotal); //envia los argumentos mediante invoke al metodo Calculo_subtotal
                            listadoEjecuciones.Add(evaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
                            listadoParaTotal.Add(respuesta); //almacena ejecucón para el calculo del subtotal
                            listadoParaSubtotal.Clear();
                            break;
                        case "total":
                            object[] argstotal = { registro, listadoParaTotal }; //carga los argumentos en un objeto
                            respuesta = (EjecucionCalculada)operadorTotal.Invoke(op, argstotal); //envia los argumentos mediante invoke al metodo Calculo_total
                            listadoEjecuciones.Add(evaluacion.SetEvaluacion((Unidad.Formato(respuesta)), evaluaciones)); //almacena cada ejecucionCalcuada en la lista pero antes ajusta el formato con la clase unidadess de medida
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
    }
}
