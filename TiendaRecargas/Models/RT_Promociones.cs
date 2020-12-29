using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    public class RT_Promociones
    {
        public int id { get; set; }
        public int semana { get; set; }
        public int year { get; set; }
        public bool activo { get; set; } = true;
    }
}
