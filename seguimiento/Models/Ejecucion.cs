using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Ejecucion
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public int idindicador { get; set; }
        [ForeignKey("idindicador")]
        public virtual Indicador Indicador { get; set; }

        [Required]
        public int idperiodo { get; set; }
        [ForeignKey("idperiodo")]
        public virtual Periodo Periodo { get; set; }

        [Display(Name = "Planeado")]
        public string planeado { get; set; }
        [Display(Name = "Ejecutado")]
        public string ejecutado { get; set; }

        [Display(Name = "Cargado")]
        public bool cargado { get; set; }

        [Display(Name = "Análisis")]
        public string Nota { get; set; }

        [Display(Name = "Ficha")]
        public string adjunto { get; set; }

        [DataType(DataType.DateTime)]
        private DateTime? updatedDate;
        [Display(Name = "F. Actualización")]
        public DateTime FechaActualizacion { get { return updatedDate ?? DateTime.UtcNow; } set { updatedDate = value; } }

        public virtual ICollection<EjecucionAdjunto> Adjuntos { get; set; }
    }
}
