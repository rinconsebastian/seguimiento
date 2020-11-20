using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Periodo
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Display(Name = "Orden")]
        [Required]
        public int orden { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Tipo")]
        [Required]
        public string tipo { get; set; }

        [Display(Name = "Calcular totales")]
        [Required]
        public bool calculo { get; set; }

        [Display(Name = "Periodo reportado")]
        [Required]
        public bool cargado { get; set; }

        [Display(Name = "Editar ejecución")]
        [Required]
        public bool EditarEjecucion { get; set; }

        [Display(Name = "Editar programación")]
        public bool EditarProgramacion { get; set; }

        [Display(Name = "Ocultar")]
        public bool Ocultar { get; set; }

        [Display(Name = "Desplegar automaticamente")]
        public bool desplegado { get; set; }

    }
}
