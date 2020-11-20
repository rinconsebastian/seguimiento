using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace seguimiento.Models
{
    public class Campo
    {
        [Required]
        [Key]
        public int Id { get; set; }



        public virtual Nivel NivelPadre { get; set; }

        public virtual TipoIndicador TipoIndicadorPadre { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Activado")]
        [Required]
        public bool Activado { get; set; }

        [Display(Name = "Todos los indicadores")]
        public bool TodoIndicador { get; set; }

        [Display(Name = "Todas las categorias")]
        public bool TodaCategoria { get; set; }


    }


}
