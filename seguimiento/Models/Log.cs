using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Log
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        public string UserMail { get; set; }

        [Display(Name = "Tarea")]
        [Required]
        public string Tarea { get; set; }

        [Required]
        [Display(Name = "Acción")]
        public string Accion { get; set; }

        [Display(Name = "Contenido nuevo")]
        public string ContenidoNew { get; set; }

        [Display(Name = "Contenido anterior")]
        public string ContenidoOld { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        private DateTime? createdDate;
        [Display(Name = "Fecha")]
        public DateTime TimeStamp { get { return createdDate ?? DateTime.UtcNow; } set { createdDate = value; } }



    }
}
