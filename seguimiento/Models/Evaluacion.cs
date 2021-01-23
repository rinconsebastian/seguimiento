using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Evaluacion
    {

        [Required]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }


        [Display(Name = "Color")]
        [Required]
        public string Color { get; set; }

        [Display(Name = "Valor Mínimo")]
        [Required]
        public decimal Minimo { get; set; }

        [Display(Name = "Valor Máximo")]
        [Required]
        public decimal Maximo { get; set; }

        [Display(Name = "Contexto")]
        [Required]
        public string Contexto { get; set; }

        [Display(Name = "Indicador")]
        public virtual Indicador Indicador { get; set; }

        [Display(Name = "Categoria")]
        public virtual Categoria Categoria { get; set; }

        [NotMapped]
        public int IdIndicador { get; set; }

        [NotMapped]
        public int IdCategoria { get; set; }

    }
}
