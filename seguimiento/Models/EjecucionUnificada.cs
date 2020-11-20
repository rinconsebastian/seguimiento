using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento.Models
{
    public class EjecucionUnificada
    {
        public int Id { get; set; }

        public decimal Valorcalculado { get; set; }

        public decimal Ponderador { get; set; }

        public bool Calculado { get; set; }

        public string Periodo { get; set; }

        public bool operar { get; set; }
        public bool cargado { get; set; }

        public string tipo { get; set; }

    }
}
