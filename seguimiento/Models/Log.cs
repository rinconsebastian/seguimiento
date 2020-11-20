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
        public string UserName { get; set; }

        [Required]
        public string UserMail { get; set; }


        [Required]
        public string Tarea { get; set; }

        [Required]
        public string Accion { get; set; }


        public string ContenidoNew { get; set; }

        public string ContenidoOld { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        private DateTime? createdDate;
        public DateTime TimeStamp { get { return createdDate ?? DateTime.UtcNow; } set { createdDate = value; } }



    }
}
