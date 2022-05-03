using Microsoft.AspNetCore.Mvc;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Formulas
{
           public class UnidadesdeMedida
        {

            public EjecucionCalculada Formato(EjecucionCalculada ejecucionOriginal)
            {
                EjecucionCalculada ejecucion = new EjecucionCalculada();
                ejecucion = ejecucionOriginal.Clone();  // se clona el objeto para evitar editar la ejecucion original

                switch (ejecucion.Indicador.unidad)
                {
                    case "Porcentaje":
                        return FormatoPorcentaje(ejecucion);
                    case "Numero":
                        return FormatoNumero(ejecucion);
                    case "NumeroSinCalculo":
                        return FormatoNumeroSinCalculo(ejecucion);
                    case "PorcentajeSinCalculo":
                        return FormatoPorcentajeSinCalculo(ejecucion);
                    default:
                        return FormatoNumero(ejecucion);

                }


            }
            private EjecucionCalculada FormatoPorcentaje(EjecucionCalculada ejecucion)
            {
                decimal valEjecutado = 0, valPlaneado = 0;
                string msg = "";

            if (ejecucion.ejecutado != null) { ejecucion.ejecutado = ejecucion.ejecutado.Replace(',', '.'); }
            if (ejecucion.planeado != null) { ejecucion.planeado = ejecucion.planeado.Replace(',', '.'); }

            try { decimal.TryParse(ejecucion.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
            catch (System.OverflowException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "ejecutado Nulo"; }

            try { decimal.TryParse(ejecucion.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
            catch (System.OverflowException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "planeado Nulo, "; }


                if (ejecucion.planeado != "" && ejecucion.planeado != null)
                {
                    if (ejecucion.PlaneadoError == null)
                    {
                        ejecucion.planeado = string.Format("{0:0.00}%", valPlaneado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                    }

                }
                else
                {
                    ejecucion.planeado = "";
                }




                switch (ejecucion.Periodo.tipo)
                {
                    case "periodo":


                        //estilo de los calculado en los periodos tipo porcentaje
                        if (ejecucion.cargado && ejecucion.Periodo.cargado)
                        {
                            if (ejecucion.EjecutadoError == null)
                            {
                                ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                            }
                        }
                        else
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        //estilo de los ejecutados en los periodos tipo porcentaje
                        if (ejecucion.ejecutado != "" && ejecucion.ejecutado != null)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }
                        if (ejecucion.EjecutadoError != null || ejecucion.PlaneadoError != null)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        break;
                    case "subtotal":
                        //estilo de los calculado en los subtotales tipo porcentaje
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                        }

                        //condicional para los valores ejecutados total y subtotal si hay un valor ejecutado pero no se ha activado el periodo no se debe mostrat
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }

                        break;
                    case "Total":


                        //estilo de los calculado en los totales tipo porcentaje
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                        }

                        //condicional para los valores ejecutados total y subtotal si hauy un valor ejecutado pero no se ha activado el periodo no se debe mostrat
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }


                        break;
                    default:
                        break;

                }




                return ejecucion;
            }
            private EjecucionCalculada FormatoNumero(EjecucionCalculada ejecucion)
            {
                decimal valEjecutado = 0, valPlaneado = 0;
                string msg = "";

            if (ejecucion.ejecutado != null) { ejecucion.ejecutado = ejecucion.ejecutado.Replace(',', '.'); }
            if (ejecucion.planeado != null) { ejecucion.planeado = ejecucion.planeado.Replace(',', '.'); }

            try { decimal.TryParse(ejecucion.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
            catch (System.OverflowException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "ejecutado Nulo"; }

            try { decimal.TryParse(ejecucion.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
            catch (System.OverflowException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "planeado Nulo, "; }


                if (ejecucion.planeado != "" && ejecucion.planeado != null)
                {
                    if (ejecucion.PlaneadoError == null)
                    {
                        ejecucion.planeado = string.Format("{0:0.0}", valPlaneado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                    }
                }
                else { ejecucion.planeado = ""; }



                switch (ejecucion.Periodo.tipo)
                {
                    case "periodo":
                        //estilo de los calculado en los periodos tipo numero
                        if (ejecucion.cargado && ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                        }
                        //estilo de los ejecutados en los periodos tipo numero
                        if (ejecucion.ejecutado != "" & ejecucion.ejecutado != null)
                        {
                            if (ejecucion.EjecutadoError == null)
                            {
                                ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                            }
                        }
                        else { ejecucion.ejecutado = ""; }

                        if (ejecucion.EjecutadoError != null || ejecucion.PlaneadoError != null)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        break;
                    case "subtotal":
                        //estilo de los calculado en los subtotales tipo numero
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                        }
                        //estilo de los ejecutados en los subtotales tipo numero
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }
                        break;
                    case "Total":

                        //estilo de los calculado en los totales tipo numero
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = string.Format("{0:0.00}%", ejecucion.Calculado);
                        }
                        //estilo de los ejecutado en los totales tipo numero
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }
                        break;
                    default:
                        break;

                }


                return ejecucion;
            }

            private EjecucionCalculada FormatoNumeroSinCalculo(EjecucionCalculada ejecucion)
            {
                decimal valEjecutado = 0, valPlaneado = 0;
                string msg = "";

            if (ejecucion.ejecutado != null) { ejecucion.ejecutado = ejecucion.ejecutado.Replace(',', '.'); }
            if (ejecucion.planeado != null) { ejecucion.planeado = ejecucion.planeado.Replace(',', '.'); }

            try { decimal.TryParse(ejecucion.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
            catch (System.OverflowException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "ejecutado Nulo"; }

            try { decimal.TryParse(ejecucion.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
            catch (System.OverflowException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "planeado Nulo, "; }


                if (ejecucion.planeado != "" && ejecucion.planeado != null)
                {
                    if (ejecucion.PlaneadoError == null)
                    {
                        ejecucion.planeado = string.Format("{0:0.0}", valPlaneado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                    }
                }
                else { ejecucion.planeado = ""; }



                switch (ejecucion.Periodo.tipo)
                {
                    case "periodo":
                        //estilo de los calculado en los periodos tipo numero
                        if (ejecucion.cargado && ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }
                        //estilo de los ejecutados en los periodos tipo numero
                        if (ejecucion.ejecutado != "" && ejecucion.ejecutado != null)
                        {
                            if (ejecucion.EjecutadoError == null)
                            {
                                ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                            }
                        }
                        else { ejecucion.ejecutado = ""; }

                        break;
                    case "subtotal":
                        //estilo de los calculado en los subtotales tipo numero
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }
                        //estilo de los ejecutados en los subtotales tipo numero
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }
                        break;
                    case "Total":

                        //estilo de los calculado en los totales tipo numero
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }
                        //estilo de los ejecutado en los totales tipo numero
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.0}", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }
                        break;
                    default:
                        break;

                }

                if (ejecucion.EjecutadoError != null || ejecucion.PlaneadoError != null)
                {
                    ejecucion.CalculadoDisplay = "";
                }

                return ejecucion;
            }
            private EjecucionCalculada FormatoPorcentajeSinCalculo(EjecucionCalculada ejecucion)
            {
                decimal valEjecutado = 0, valPlaneado = 0;
                string msg = "";

            if (ejecucion.ejecutado != null) { ejecucion.ejecutado = ejecucion.ejecutado.Replace(',', '.'); }
            if (ejecucion.planeado != null) { ejecucion.planeado = ejecucion.planeado.Replace(',', '.'); }

            try { decimal.TryParse(ejecucion.ejecutado, NumberStyles.Any, CultureInfo.InvariantCulture, out valEjecutado); }
            catch (System.OverflowException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "el valor ejecutado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.EjecutadoError = ejecucion.EjecutadoError + "ejecutado Nulo"; }

            try { decimal.TryParse(ejecucion.planeado, NumberStyles.Any, CultureInfo.InvariantCulture, out valPlaneado); }
            catch (System.OverflowException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado genera desbordamiento, "; }
                catch (System.FormatException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "el valor planeado tiene un formato incorrecto, "; }
                catch (System.ArgumentNullException) { ejecucion.PlaneadoError = ejecucion.PlaneadoError + "planeado Nulo, "; }



                if (ejecucion.planeado != "" && ejecucion.planeado != null)
                {
                    if (ejecucion.PlaneadoError == null)
                    {
                        ejecucion.planeado = string.Format("{0:0.00}%", valPlaneado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                    }
                }
                else
                {
                    ejecucion.planeado = "";
                }




                switch (ejecucion.Periodo.tipo)
                {
                    case "periodo":


                        //estilo de los calculado en los periodos tipo porcentaje
                        if (ejecucion.cargado && ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        //estilo de los ejecutados en los periodos tipo porcentaje
                        if (ejecucion.ejecutado != "" && ejecucion.ejecutado != null)
                        {
                            if (ejecucion.EjecutadoError == null)
                            {
                                ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                            }
                        }
                        else { ejecucion.ejecutado = ""; }

                        break;
                    case "subtotal":
                        //estilo de los calculado en los subtotales tipo porcentaje
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        //condicional para los valores ejecutados total y subtotal si hay un valor ejecutado pero no se ha activado el periodo no se debe mostrat
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }

                        break;
                    case "Total":


                        //estilo de los calculado en los totales tipo porcentaje
                        if (ejecucion.Periodo.cargado)
                        {
                            ejecucion.CalculadoDisplay = "";
                        }

                        //condicional para los valores ejecutados total y subtotal si hauy un valor ejecutado pero no se ha activado el periodo no se debe mostrat
                        if (ejecucion.ejecutado != "" && ejecucion.Periodo.cargado)
                        {
                            ejecucion.ejecutado = string.Format("{0:0.00}%", valEjecutado); //agrega el signo % a todos los planeados no nulos con unidad porcentaje
                        }
                        else
                        {
                            ejecucion.ejecutado = "";
                        }


                        break;
                    default:
                        break;

                }

                if (ejecucion.EjecutadoError != null || ejecucion.PlaneadoError != null)
                {
                    ejecucion.CalculadoDisplay = "";
                }

                return ejecucion;
            }


        }
    }

