using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    public class Facturacion
    {
        public decimal total { get; set; } = 0;
        public string semana { get; set; }
        public int year { get; set; }
        public List<Recarga> Recargas { get; set; } = new List<Recarga>();
        public List<Cuenta> cuentas { get; set; } = new List<Cuenta>();
    }
}
