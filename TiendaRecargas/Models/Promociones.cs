using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Extensions;

namespace TiendaRecargas.Models
{
    public class Promociones
    {
        public int id { get; set; }
        [Required]
        public string semana { get; set; }
        public int year { get; set; }
        [Required]
        public string texto { get; set; }
        public bool activo { get; set; } = true;

        [NotMapped]
        public string primerDiaPromocion
        {
            get
            {
                return semana.FirstDateOfWeek().AddDays(1).ToEasternStandardTime().ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            }
        }
    }
}
