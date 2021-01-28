using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class IndicadorChartViewModel
    {
        public string Periodo { get; set; }
        
        public decimal? Planeado { get; set; }
        public decimal? Ejecutado { get; set; }
    }

}
