using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class ReporteViewModel

    {
        public Indicador Indicador { get; set; }
        public List<Ejecucion> Ejecuciones { get; set; }
    }
}
