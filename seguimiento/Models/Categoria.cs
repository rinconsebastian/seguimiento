using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class Categoria
    {
        [Required]
        [Key]
        public int id { get; set; }

        [JsonIgnore]
        [Display(Name = "Categoría padre")]
        public int? idCategoria { get; set; }
        [JsonIgnore]
        [ForeignKey("idCategoria")]
        public virtual Categoria CategoriaPadre { get; set; }

        [Required]
        [Display(Name = "Nivel")]
        public int idNivel { get; set; }
        [ForeignKey("idNivel")]
        public virtual Nivel Nivel { get; set; }

        [JsonIgnore]
        [Display(Name = "Responsable")]
        [Required]
        public int IdResponsable { get; set; }
        [JsonIgnore]
        [ForeignKey("IdResponsable")]
        public virtual Responsable Responsable { get; set; }

        [Display(Name = "Numero")]
        public string numero { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Calcular valor de la categoría")]
        [Required]
        public bool unificacion { get; set; }

        [Display(Name = "Ponderador")]
        [Required]
        public decimal Ponderador { get; set; }

        [Display(Name = "Texto")]
        public string texto { get; set; }

        [Display(Name = "Objetivo")]
        public string objetivo { get; set; }

        [Display(Name = "Nota")]
        public string nota { get; set; }

        public virtual ICollection<Indicador> Indicadores { get; set; }

    }
}
