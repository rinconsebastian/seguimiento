using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class Nivel
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Numero")]
        public int numero { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string color { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }


    }
}
