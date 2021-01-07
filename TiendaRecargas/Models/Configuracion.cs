using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    public class Configuracion
    {
        public int id { get; set; }
        [Required]
        public decimal baseCalculoPorciento { get; set; }
        [Required]
        public decimal tasaCambioCUP { get; set; }
    }
}
