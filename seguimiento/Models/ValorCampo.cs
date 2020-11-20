using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace seguimiento.Models
{
    public class ValorCampo
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public int IdCampo { get; set; }
        [ForeignKey("IdCampo")]
        public virtual Campo CampoPadre { get; set; }


        public virtual Indicador IndicadorPadre { get; set; }
        public virtual Categoria CategoriaPadre { get; set; }

        public string Texto { get; set; }

    }
}
