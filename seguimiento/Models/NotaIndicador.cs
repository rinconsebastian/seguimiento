using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace seguimiento.Models
{
    public class NotaIndicador
    {
        [Required]
        [Key]
        public int Id { get; set; }


        [Display(Name = "Título")]
        [Required]
        public string Titulo { get; set; }

        [Display(Name = "Detalle")]
        [Required]
        public string Texto { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        private DateTime? createdDate;
        [Display(Name = "F. creación")]
        public DateTime FechaCreacion { get { return createdDate ?? DateTime.UtcNow; } set { createdDate = value; } }


        [Required]
        [DataType(DataType.DateTime)]
        private DateTime? updatedDate;
        [Display(Name = "F. Actualización")]
        public DateTime FechaActualizacion { get { return updatedDate ?? DateTime.UtcNow; } set { updatedDate = value; } }



        [DataType(DataType.ImageUrl)]
        public string Adjunto { get; set; }

        [Display(Name = "Indicador")]
        [Required]
        public int IdIndicador { get; set; }
        [ForeignKey("IdIndicador")]
        public virtual Indicador Indicador { get; set; }

        [Display(Name = "Usuario")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


    }
}
