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

        public int totalRecargas
        {
            get
            {
                var recargasPadre = 0;
                recargasPadre += Recargas.Count;
                recargasPadre += cuentas.Sum(x => x.Recargas.Count);
                return recargasPadre;
            }
        }

        public decimal montoTotalRecargas
        {
            get
            {
                decimal costoTotal = 0;
                costoTotal += Recargas.Sum(x => x.monto);
                costoTotal += cuentas.Sum(x => x.Recargas.Sum(r => r.monto));
                return costoTotal;
            }
        }

        public List<Recarga> Recargas { get; set; } = new List<Recarga>();
        public List<Cuenta> cuentas { get; set; } = new List<Cuenta>();
    }
}
