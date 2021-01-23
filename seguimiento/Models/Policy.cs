using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace seguimiento.Models
{
    public class Policy
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Claim")]
        public string claim { get; set; }
    }
}
