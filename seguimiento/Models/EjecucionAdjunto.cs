using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class EjecucionAdjunto
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public int idejecucion { get; set; }
        [ForeignKey("idejecucion")]
        public virtual Ejecucion Ejecucion { get; set; }

        public string nombre { get; set; }

        [Required]
        [Display(Name = "Ficha")]
        public string adjunto { get; set; }

    }
}
