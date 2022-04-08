using Microsoft.AspNetCore.Identity;
using seguimiento.Controllers;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace seguimiento.Formulas
{
    public class PonderacionAbsoluta
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public PonderacionAbsoluta(ApplicationDbContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }
        public async Task<string> Calculo_total_categoria(Configuracion configuracion)
        {
            //controladores
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);                              //Instancia controlador de EjecucionesCategorias
            NivelsController controlnivel = new NivelsController(db,userManager);                                                                     //Instancia controlador de Niveles
            CategoriasController controlCategorias = new CategoriasController(db, userManager);                                                        //Instancia controlador de Categorias
            PeriodosController controlPeriodos = new PeriodosController(db, userManager);                                                              //Instancia controlador de Periodos

            // variables
            List<Periodo> periodos =await controlPeriodos.getAll();                                  //Obtiene los periodos
            Nivel nivelMaximo =await controlnivel.getMain();                                         //obtienen la categoria principal
            int nivelaOperar = System.Convert.ToInt32(configuracion.CalculoNivel);              //obtiene la categoria de la que se tomaran los indicadores a operar
            int MinNivelaOperar = nivelaOperar;

            //------------------------------------------------------------inicia con las categorias de l nivel en el que estan los indicadores

            var categoriasz = await controlCategorias.getFromNivel(nivelaOperar);                     //obtiene categorias de menor nivel 
            controlEjecucionCategoria.BorrarTodo();                                             //borra la base de datos
            var resp = await Calcular_categoria_menor(categoriasz, periodos);                                              // calcula la categoria de menor nivel

            nivelaOperar = nivelaOperar - 1;

            while (nivelaOperar >= nivelMaximo.numero)
            {
                categoriasz = await controlCategorias.getFromNivel(nivelaOperar);
                await Calcular_categorias_nivel_n(categoriasz, periodos, MinNivelaOperar);
                nivelaOperar = nivelaOperar - 1;
            }



            var carro = "mio";
            return carro;
        }

        private async Task<bool> Calcular_categoria_menor(List<Categoria> categoriasz, List<Periodo> periodos)
        {
            bool resp = false;

            //controladores
            IndicadorsController controlIndicadores = new IndicadorsController(db, userManager);
            EjecucionsController controlEjecucion = new EjecucionsController(db, userManager);
            PeriodosController controlPeriodos = new PeriodosController(db, userManager);
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);

            // variables
            List<Indicador> indicadores = new List<Indicador>();
            List<Ejecucion> ejecuciones = new List<Ejecucion>();
            List<EjecucionCalculada> ejecucionesCalculadas = new List<EjecucionCalculada>();
            //List<Periodo> periodos = controlPeriodos.getAll();

            foreach (Categoria categoriax in categoriasz)
            {


                indicadores = await controlIndicadores.getFromCategoria(categoriax.id);
                List<EjecucionCalculada> listadoEjecuciones = new List<EjecucionCalculada>();
                List<EjecucionCategoria> respuestas = new List<EjecucionCategoria>();

                foreach (Indicador indicador in indicadores)
                {

                    //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
                    ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                    Object op = manejador.Unwrap();
                    Type t = op.GetType();
                    MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
                    MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
                    MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total
                    decimal lineaBase = 0;
                    EjecucionCalculada respuesta = new EjecucionCalculada();

                    List<object> listadoParaSubtotal = new List<object>();
                    List<object> listadoParaTotal = new List<object>();
                    string msg = "";


                    ejecuciones = await controlEjecucion.getFromIndicador(indicador.id);
                    foreach (Ejecucion registro in ejecuciones)
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
                                Object[] argsSubtotal = { registro, listadoParaSubtotal, lineaBase }; //carga los argumentos en un objeto 
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
                                    lb = lb.Replace(" ", "");
                                    lb = lb.Replace("%", "");
                                    lb = Regex.Replace(lb, "^-$", "");
                                    lb = Regex.Replace(lb, "^_$", "");
                                    lb = Regex.Replace(lb, "[a-zA-Z^&()<>//:@#$%;+_!¡]", "");
                                }

                                try { lineaBase = lb == "" ? 0 : System.Convert.ToDecimal(lb); }
                                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }

                                break;
                            default:

                                break;

                        }
                    }


                }

                foreach (Periodo periodo in periodos)
                {
                    decimal total = 0, calculado = 0;
                    bool mostrar = false;

                    var ejecucionesperiodo =
                        (from ejecucionx in listadoEjecuciones
                         where ejecucionx.Periodo.id == periodo.id
                         select ejecucionx);

                    foreach (EjecucionCalculada calcuada in ejecucionesperiodo)
                    {
                        if ((calcuada.EjecutadoError == null && calcuada.PlaneadoError == null && calcuada.cargado == true) || calcuada.Periodo.tipo != "periodo")
                        {
                            total = total + calcuada.Indicador.ponderador;
                            calculado = calculado + (calcuada.Calculado / 100) * calcuada.Indicador.ponderador;
                        }
                    }
                    if (total > 0)
                    {
                        calculado = (calculado / total) * 100;
                        mostrar = true; 
                    }
                    else
                    {
                        calculado = 0;
                    }

                    EjecucionCategoria resultado = new EjecucionCategoria();
                    //resultado.Id = 1;
                    resultado.Calculado = calculado;
                    //resultado.Categoria = categoriax;
                    resultado.IdCategoria = categoriax.id;
                    resultado.idperiodo = periodo.id;
                    resultado.Maximo = total;
                    resultado.Mostrar = mostrar;

                    var r = await controlEjecucionCategoria.Crear(resultado);





                }


            }
            return resp;
        }

        private async Task<bool> Calcular_categorias_nivel_n(List<Categoria> categoriasz, List<Periodo> periodos, int nivelaOperar)
        {

            //controladores
            IndicadorsController controlIndicadores = new IndicadorsController(db,userManager);
            EjecucionsController controlEjecucion = new EjecucionsController(db,userManager);
            PeriodosController controlPeriodos = new PeriodosController(db, userManager);
            CategoriasController controlCategorias = new CategoriasController(db, userManager);
            EjecucionCategoriaController controlEjecucionCategoria = new EjecucionCategoriaController(db, userManager);


            // variables
            List<Indicador> indicadores = new List<Indicador>();
            List<Ejecucion> ejecuciones = new List<Ejecucion>();
            List<EjecucionCalculada> ejecucionesCalculadas = new List<EjecucionCalculada>();
            //List<Periodo> periodos = controlPeriodos.getAll();


            foreach (Categoria categoriax in categoriasz)
            {

                List<Categoria> CategoriasPAraIndicadores = await controlCategorias.GetFomNivelAndParent(categoriax.id, nivelaOperar);

                indicadores = await controlIndicadores.getFromCategorias(CategoriasPAraIndicadores);

                List<EjecucionCalculada> listadoEjecuciones = new List<EjecucionCalculada>();
                List<EjecucionCategoria> respuestas = new List<EjecucionCategoria>();

                foreach (Indicador indicador in indicadores)
                {

                    //-------------- generacion de un objeto genérico para manejar los diferentes tipos de indicadores
                    ObjectHandle manejador = Activator.CreateInstance(null, "seguimiento.Formulas." + indicador.TipoIndicador.file); //se crea un manejador  op -objeto generico- y un operador generico que permite llamar a las formulas con la cadena del tipo de indiciador: mantenimiento, incremento etc
                    Object op = manejador.Unwrap();
                    Type t = op.GetType();
                    MethodInfo operadorPeriodo = t.GetMethod("Calculo_periodo"); //operador es un metodo generico que refleja la funcionalidad de Calculo periodo
                    MethodInfo operadorSubtotal = t.GetMethod("Calculo_subtotal"); //operador es un metodo generico que refleja la funcionalidad de Calculo subtotal
                    MethodInfo operadorTotal = t.GetMethod("Calculo_total"); //operador es un metodo generico que refleja la funcionalidad de Calculo total
                    decimal lineaBase = 0;
                    EjecucionCalculada respuesta = new EjecucionCalculada();

                    List<object> listadoParaSubtotal = new List<object>();
                    List<object> listadoParaTotal = new List<object>();
                    string msg = "";


                    ejecuciones =await controlEjecucion.getFromIndicador(indicador.id);
                    foreach (Ejecucion registro in ejecuciones)
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
                                Object[] argsSubtotal = { registro, listadoParaSubtotal, lineaBase }; //carga los argumentos en un objeto 
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
                                    lb = lb.Replace(" ", "");
                                    lb = lb.Replace("%", "");
                                    lb = Regex.Replace(lb, "^-$", "");
                                    lb = Regex.Replace(lb, "^_$", "");
                                    lb = Regex.Replace(lb, "[a-zA-Z^&()<>//:@#$%;+_!¡]", "");
                                }

                                try { lineaBase = lb == "" ? 0 : System.Convert.ToDecimal(lb); }
                                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                                break;
                            default:

                                break;

                        }
                    }


                }

                foreach (Periodo periodo in periodos)
                {
                    decimal total = 0, calculado = 0;
                    bool mostrar = false;

                    var ejecucionesperiodo =
                        (from ejecucionx in listadoEjecuciones
                         where ejecucionx.idperiodo == periodo.id
                         select ejecucionx);

                    foreach (EjecucionCalculada calcuada in ejecucionesperiodo)
                    {
                        total = total + calcuada.Indicador.ponderador;
                        if (calcuada.cargado == true || ((calcuada.Periodo.tipo == "subtotal" || calcuada.Periodo.tipo == "total") && calcuada.Periodo.cargado == true))
                        {
                            calculado = calculado + (calcuada.Calculado / 100) * calcuada.Indicador.ponderador;
                        }
                    }
                    if (total > 0)
                    {
                        calculado = (calculado / total) * 100;
                        mostrar = true;
                    }
                    else
                    {
                        calculado = 0;
                    }

                    EjecucionCategoria resultado = new EjecucionCategoria();
                    //resultado.Id = 1;
                    resultado.Calculado = calculado;
                    //resultado.Categoria = categoriax;
                    resultado.IdCategoria = categoriax.id;
                    resultado.idperiodo = periodo.id;
                    resultado.Maximo = total;
                    //resultado.Periodo = periodo;
                    resultado.Mostrar = mostrar;

                    var r = await controlEjecucionCategoria.Crear(resultado);





                }


            }

            return false;
        }

        public async Task<decimal> CategoriaPonderador(int categoria)
        {
            CategoriasController controlCategoria = new CategoriasController(db, userManager);
            IndicadorsController controlIndicador = new IndicadorsController(db, userManager);
            decimal ponderador = 0;
            Dictionary<int,int> categorias =await controlCategoria.CategoriasMenores(categoria);

            foreach (var categ in categorias)
            {
                List<Indicador> indicadores = await controlIndicador.getFromCategoria(categ.Key);
                foreach (Indicador ind in indicadores)
                {
                    ponderador = ponderador + ind.ponderador;
                }
            }

            return ponderador;
        }






    }

}

