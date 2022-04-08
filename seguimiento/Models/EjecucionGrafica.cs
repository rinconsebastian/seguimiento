using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class EjecucionGrafica
    {
       

        [Display(Name = "Planeado")]
        public string Planeado { get; set; }


        [Display(Name = "Ejecutado")]
        public string Ejecutado { get; set; }



        [Display(Name = "Periodo")]
        public string Periodo { get; set; }

    }
}
