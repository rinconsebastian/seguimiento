using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Indicador
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public int idCategoria { get; set; }
        [Display(Name = "Categoria")]
        [ForeignKey("idCategoria")]
        public virtual Categoria Categoria { get; set; }

        [Required]
        [Display(Name = "Ponderador")]
        public Decimal ponderador { get; set; }

        [Required]
        [Display(Name = "Dinámica")]
        public int tipo { get; set; }

        [Display(Name = "Dinámica")]
        [ForeignKey("tipo")]
        public virtual TipoIndicador TipoIndicador { get; set; }

        [Display(Name = "Número")]
        public string codigo { get; set; }

        [Required]
        [Display(Name = "Unidad")]
        public string unidad { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Nota")]
        public string Nota { get; set; }

        [Display(Name = "Ficha")]
        public string adjunto { get; set; }


    }
}
