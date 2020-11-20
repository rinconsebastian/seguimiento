using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class EjecucionCalculada : Ejecucion
    {

        [Required]
        public decimal Calculado { get; set; }


        [Required]
        public string Mensaje { get; set; }

        [Display(Name = "complimiento")]
        public string CalculadoDisplay { get; set; }

        public string PlaneadoError { get; set; }
        public string EjecutadoError { get; set; }

        public EvaluacionDisplay Evaluacion { get; set; }




        public EjecucionCalculada Clone() // es necesario definir un metodo de clonacion para evitar modificar las cadenas de ejecucion y planeado al agregar%
        {
            return (EjecucionCalculada)this.MemberwiseClone();
        }


    }
}
