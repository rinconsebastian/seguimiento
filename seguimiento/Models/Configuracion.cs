using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Configuracion
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public int anoInicial { get; set; }

        [Required]
        public int anoFinal { get; set; }


        [Required]
        public int periodosAnuales { get; set; }


        [Required]
        public string nombrePeriodoAnual { get; set; }


        public string Logo { get; set; }

        public string ImgHeader { get; set; }

        public string ImgBackgroud { get; set; }

        [Required]
        public string contacto { get; set; }

        [Required]
        public bool activo { get; set; }

        [Required]
        public string Entidad { get; set; }

        [Required]
        public string NombrePlan { get; set; }

        [Required]
        public string PonderacionTipo { get; set; }

        [Required]
        public string CalculoNivel { get; set; }

        [Required]
        public bool libre { get; set; }

    }
}
