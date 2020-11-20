using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class Categoria
    {
        [Required]
        [Key]
        public int id { get; set; }


        public int idCategoria { get; set; }
        [ForeignKey("idCategoria")]
        public virtual Categoria CategoriaPadre { get; set; }

        [Required]
        public int idNivel { get; set; }
        [ForeignKey("idNivel")]
        public virtual Nivel Nivel { get; set; }


        public int IdResponsable { get; set; }
        [ForeignKey("IdResponsable")]
        public virtual Responsable Responsable { get; set; }


        public string numero { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Calcular valor de la categoria")]
        [Required]
        public bool unificacion { get; set; }


        [Required]
        public decimal Ponderador { get; set; }


        public string texto { get; set; }

        public string objetivo { get; set; }


        public string nota { get; set; }

    }
}
