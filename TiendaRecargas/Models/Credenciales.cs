using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    public class Credenciales
    {
        public int id { get; set; }
        [Required]
        public string proveedor { get; set; }
        [Required]
        public string codigo { get; set; }
        [Required]
        public string token { get; set; }

        public int idCuenta { get; set; }
    }
}
