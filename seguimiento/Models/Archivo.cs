using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seguimiento.Models
{
    public class Archivo
    {
        public bool Loaded { get; set; }

        public string Nombre { get; set; }

        public string Ruta { get; set; }


    }
}
