using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class EjecucionCategoria
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

        [Required]
        public int idperiodo { get; set; }
        [ForeignKey("idperiodo")]
        public virtual Periodo Periodo { get; set; }

        public decimal Calculado { get; set; }
        public decimal Maximo { get; set; }
        public bool Mostrar { get; set; }



    }
}
