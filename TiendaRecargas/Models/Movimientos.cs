using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Extensions;

namespace TiendaRecargas.Models
{
    public class Movimientos
    {

        public int id { get; set; }
        public int idUserLogged { get; set; }
        public int idCuenta { get; set; }
        public decimal? oldValue { get; set; }
        public decimal newValue { get; set; }
        public DateTime fecha { get; set; } = DateTime.Now.ToEasternStandardTime();
        public string tipo { get; set; }
    }
}
