using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Sesion
    {
        [Required]
        public ApplicationUser usuario { get; set; }

        [Required]
        public Responsable responsable { get; set; }

        [Required]
        public Configuracion configuracion { get; set; }

        [Required]
        public PermisoRol permisos { get; set; }


    }
}
