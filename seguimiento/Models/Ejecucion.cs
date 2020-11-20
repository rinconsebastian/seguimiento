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

        public string planeado { get; set; }

        public string ejecutado { get; set; }

        public bool cargado { get; set; }

        [Display(Name = "Análisis")]
        public string Nota { get; set; }

        public string adjunto { get; set; }




        [DataType(DataType.DateTime)]
        private DateTime? updatedDate;
        [Display(Name = "F. Actualización")]
        public DateTime FechaActualizacion { get { return updatedDate ?? DateTime.UtcNow; } set { updatedDate = value; } }

    }
}
