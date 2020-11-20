using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class TipoIndicador
    {
        [Required]
        [Key]
        public int Id { get; set; }


        [Required]
        public string Tipo { get; set; }

        [Required]
        public string file { get; set; }

        [Required]
        public bool Enable { get; set; }


        public string Descripcion { get; set; }

        public string Imagendescripcion { get; set; }




    }
}
