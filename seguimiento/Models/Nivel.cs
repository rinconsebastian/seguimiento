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
        public int numero { get; set; }

        [Required]
        public string color { get; set; }

        [Display(Name = "Nivel")]
        [Required]
        public string nombre { get; set; }


    }
}
