using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class CategoriaEjecucion
    {

        public int IdCategoria { get; set; }
        public int IdCategoriaPadre { get; set; }
        public int IdPeriodo { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorMaximo { get; set; }


    }
}