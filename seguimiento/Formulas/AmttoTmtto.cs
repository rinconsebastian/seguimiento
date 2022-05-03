using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;


//LIBRERIA QUE CALCULA LAS EJECUCIONES DE INDICADORES TIPO MANTENIMIENTO TANTO POR PERIODO COMO POR SUBTOTAL Y TOTAL


namespace seguimiento.Formulas
{
    public class AmttoTmtto
    {
        //eta función toma un modelo ejecucion y agrega el valor calculado y un mensaje con posibles errores y los almacena en un tipo de dato ejecucionCalculada
        public EjecucionCalculada Calculo_periodo(Ejecucion ejecucion, decimal lineaBase)
        {
            decimal valEjecutado = 0, valPlaneado = 0, valCalculado;
            string msg = "";
            EjecucionCalculada respuesta = new EjecucionCalculada();
            if (ejecucion.ejecutado != null) { ejecucion.ejecutado = ejecucion.ejecutado.Replace(',', '.'); }
            if (ejecucion.planeado != null) { ejecucion.planeado = ejecucion.planeado.Replace(',', '.'); }


            //conversión a # de los valores ejecutados y planeados
            try { decimal.TryParse(ejecucion.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
            catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
            catch (System.FormatException) { msg = "el valor ejecutado tiene un formato incorrecto"; }
            catch (System.ArgumentNullException) { msg = "ejecutado Nulo"; }

            //conversión a # de los valores ejecutados y planeados
            try { decimal.TryParse(ejecucion.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
            catch (System.OverflowException) { msg = "el valor planeado genera desbordamiento"; }
            catch (System.FormatException) { msg = "el valor planeado tiene un formato incorrecto"; }
            catch (System.ArgumentNullException) { msg = "planeado Nulo"; }
            //mensaje de no error
            if (msg == "")
            {
                //evita planeados negativos y 0 para evitar divisiones por 0, si es aso genera mensaje de error
                if (valPlaneado <= 0)
                {
                    msg = "ejecución no planeada";
                    if (valEjecutado >= valPlaneado)
                    {
                        valCalculado = 100;
                    }
                    else { valCalculado = 0; }

                }
                else
                {
                    //realiza el calculo del valor a devolver, siempre en porcentaje
                    valCalculado = ((valEjecutado / valPlaneado) * 100);
                }

                respuesta.Calculado = valCalculado;
            }

            if (respuesta.Calculado > 100) { respuesta.Calculado = 100; }
            if (respuesta.Calculado < 0) { respuesta.Calculado = 0; }

            respuesta.id = ejecucion.id;
            respuesta.FechaActualizacion = ejecucion.FechaActualizacion;
            respuesta.idindicador = ejecucion.idindicador;
            respuesta.Indicador = ejecucion.Indicador;
            respuesta.idperiodo = ejecucion.idperiodo;
            respuesta.Periodo = ejecucion.Periodo;
            respuesta.cargado = ejecucion.cargado;
            respuesta.ejecutado = ejecucion.ejecutado;
            respuesta.Nota = ejecucion.Nota;
            respuesta.adjunto = ejecucion.adjunto;
            respuesta.Mensaje = msg;
            respuesta.planeado = ejecucion.planeado;

            return respuesta;
        }

        public Object Calculo_subtotal(Ejecucion ejecucion, List<object> listadoParaSubtotal, decimal lineaBase)
        {
            decimal sumaPlaneado = 0, sumaEjecutados = 0, valEjecutado = 0, valPlaneado = 0, valCalculado = 0;
            int cuenta = 0, cuentaejecutado = 0;
            EjecucionCalculada respuesta = new EjecucionCalculada();
            string msg = "";

            foreach (Ejecucion calculada in listadoParaSubtotal)
            {
                valEjecutado = 0;
                valPlaneado = 0;

                if (calculada.ejecutado != null) { calculada.ejecutado = calculada.ejecutado.Replace(',', '.'); }
                if (calculada.planeado != null) { calculada.planeado = calculada.planeado.Replace(',', '.'); }

                try { decimal.TryParse(calculada.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                catch (System.FormatException) { msg = "el valor ejecutado tiene un formato incorrecto"; }
                catch (System.ArgumentNullException) { msg = "ejecutado Nulo"; }

                try { decimal.TryParse(calculada.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                catch (System.FormatException) { msg = "el valor ejecutado tiene un formato incorrecto"; }
                catch (System.ArgumentNullException) { msg = "ejecutado Nulo"; }

                sumaPlaneado = sumaPlaneado + valPlaneado;
                if (calculada.cargado == true && calculada.Periodo.cargado == true)
                {
                    sumaEjecutados = sumaEjecutados + valEjecutado;
                    cuentaejecutado++;
                }
                cuenta++;
            }

            if (cuenta > 0)
            {
                if (cuentaejecutado > 0)
                {
                    sumaEjecutados = sumaEjecutados / cuenta;
                }
               
                sumaPlaneado = sumaPlaneado / cuenta;


                if (sumaPlaneado > 0)
                {
                    valCalculado = sumaEjecutados / sumaPlaneado;

                    if (valCalculado > 1)
                    {
                        valCalculado = 1;
                    }
                    if (valCalculado < 0)
                    {
                        valCalculado = 0;
                    }
                }
                else if (sumaPlaneado <= 0)
                {

                    if (sumaEjecutados >= valCalculado)
                    {
                        valCalculado = 100;
                    }
                    else { valCalculado = 0; }
                }
                else
                {
                    valCalculado = 0;
                }
            }
            respuesta.id = ejecucion.id;
            respuesta.FechaActualizacion = ejecucion.FechaActualizacion;
            respuesta.idindicador = ejecucion.idindicador;
            respuesta.Indicador = ejecucion.Indicador;
            respuesta.idperiodo = ejecucion.idperiodo;
            respuesta.Periodo = ejecucion.Periodo;
            respuesta.cargado = ejecucion.cargado;
            respuesta.ejecutado = sumaEjecutados.ToString();
            respuesta.planeado = sumaPlaneado.ToString();
            respuesta.Nota = ejecucion.Nota;
            respuesta.adjunto = ejecucion.adjunto;
            respuesta.Mensaje = msg;
            respuesta.Calculado = (valCalculado * 100);


            if (respuesta.Calculado > 100) { respuesta.Calculado = 100; }
            if (respuesta.Calculado < 0) { respuesta.Calculado = 0; }

            return (Object)respuesta;
        }
        public EjecucionCalculada Calculo_total(Ejecucion ejecucion, List<object> listadoParaTotal, decimal lineaBase)
        {
            decimal valEjecutado = 0, valPlaneado = 0, valCalculado = 0, sumaPlaneado = 0, sumaEjecutado = 0;
            int cuenta = 0, cuenta2 = 0;
            EjecucionCalculada respuesta = new EjecucionCalculada();
            string msg = "";

            foreach (EjecucionCalculada calculada in listadoParaTotal)
            {

                valEjecutado = 0;
                valPlaneado = 0;
                try { valEjecutado = System.Convert.ToDecimal(calculada.ejecutado); }
                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                catch (System.FormatException) { msg = "el valor ejecutado tiene un formato incorrecto"; }
                catch (System.ArgumentNullException) { msg = "ejecutado Nulo"; }

                try { valPlaneado = System.Convert.ToDecimal(calculada.planeado); }
                catch (System.OverflowException) { msg = "el valor ejecutado genera desbordamiento"; }
                catch (System.FormatException) { msg = "el valor ejecutado tiene un formato incorrecto"; }
                catch (System.ArgumentNullException) { msg = "ejecutado Nulo"; }

                if (calculada.Periodo.cargado == true)
                {
                    valCalculado = calculada.Calculado + valCalculado;
                }
                cuenta++;


            }


            if (cuenta > 0)
            {



                valCalculado = valCalculado / cuenta;

                if (valCalculado > 100)
                {
                    valCalculado = 100;
                }
                if (valCalculado < 0)
                {
                    valCalculado = 0;
                }


            }


            respuesta.id = ejecucion.id;
            respuesta.FechaActualizacion = ejecucion.FechaActualizacion;
            respuesta.idindicador = ejecucion.idindicador;
            respuesta.Indicador = ejecucion.Indicador;
            respuesta.idperiodo = ejecucion.idperiodo;
            respuesta.Periodo = ejecucion.Periodo;
            respuesta.cargado = ejecucion.cargado;
            respuesta.ejecutado = "";
            respuesta.planeado = "";
            respuesta.Nota = ejecucion.Nota;
            respuesta.adjunto = ejecucion.adjunto;
            respuesta.Mensaje = msg;
            respuesta.Calculado = (valCalculado);


            if (respuesta.Calculado > 100) { respuesta.Calculado = 100; }
            if (respuesta.Calculado < 0) { respuesta.Calculado = 0; }

            return respuesta;
        }




    }
}