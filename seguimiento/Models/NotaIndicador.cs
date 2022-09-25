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


        //CAMPOS PROPIOS CCS
        // =========== PLAN DE ACCIÓN =========
        //-------------- 1 -------------------- 
        [Display(Name = "Acción 1")]
        public string Accion1 { get; set; }

        [Display(Name = "Responsable 1")]
        public string Responsable1 { get; set; }
        
        [Display(Name = "Fecha de inicio 1")]
        [DataType(DataType.Date)]
         public DateTime? FechaInicio1 { get; set; }
        
        [Display(Name = "Fecha final 1")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinal1 { get; set; }

        //-------------- 2 -------------------- 
        [Display(Name = "Acción 2")]
        public string Accion2 { get; set; }

        [Display(Name = "Responsable 2")]
        public string Responsable2 { get; set; }

        [Display(Name = "Fecha de inicio 2")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio2 { get; set; }

        [Display(Name = "Fecha final 2")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinal2 { get; set; }

        //-------------- 3 -------------------- 
        [Display(Name = "Acción 3")]
        public string Accion3 { get; set; }

        [Display(Name = "Responsable 3")]
        public string Responsable3 { get; set; }

        [Display(Name = "Fecha de inicio 3")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio3 { get; set; }

        [Display(Name = "Fecha final 3")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinal3 { get; set; }

        // =========== SEGUIMIENTO =========
        //-------------- 1 -------------------- 
        [Display(Name = "Fecha 1")]
        [DataType(DataType.Date)]
        public DateTime? FechaSeg1 { get; set; }

        [Display(Name = "Descripción 1")]
        public string Descripcion1 { get; set; }

        [Display(Name = "Soporte 1")]
        public string Soporte1 { get; set; }

        [Display(Name = "Cerrada 1")]
        
        public bool Cerrada1 { get; set; }
        
        //-------------- 2 -------------------- 
        [Display(Name = "Fecha 2")]
        [DataType(DataType.Date)]
        public DateTime? FechaSeg2 { get; set; }

        [Display(Name = "Descripción 2")]
        public string Descripcion2 { get; set; }

        [Display(Name = "Soporte 2")]
        public string Soporte2 { get; set; }

        [Display(Name = "Cerrada 2")]

        public bool Cerrada2 { get; set; }

        //-------------- 3 -------------------- 
        [Display(Name = "Fecha 3")]
        [DataType(DataType.Date)]
        public DateTime? FechaSeg3 { get; set; }

        [Display(Name = "Descripción 3")]
        public string Descripcion3 { get; set; }

        [Display(Name = "Soporte 3")]
        public string Soporte3 { get; set; }

        [Display(Name = "Cerrada 3")]

        public bool Cerrada3 { get; set; }

    }
}
