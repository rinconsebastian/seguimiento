using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class PermisoRol
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdRol { get; set; }


        public bool SuperUsuario { get; set; }
        public bool SuperCategoria { get; set; }

        public bool NotasV { get; set; }
        public bool NotasE { get; set; }
        public bool EjecucionesV { get; set; }
        public bool EjecucionesE { get; set; }
        public bool CategoriasV { get; set; }
        public bool CategoriasE { get; set; }
        public bool PlaneadosV { get; set; }
        public bool PlaneadosE { get; set; }
        public bool IndicadoresV { get; set; }
        public bool IndicadoresE { get; set; }
        public bool UsuariosV { get; set; }
        public bool UsuariosE { get; set; }
        public bool RolesV { get; set; }
        public bool RolesE { get; set; }
        public bool NivelesE { get; set; }
        public bool ConfiguraciongE { get; set; }
        public bool PeriodosE { get; set; }
        public bool ResponsablesE { get; set; }
        public bool NotificaconesE { get; set; }


    }
}
