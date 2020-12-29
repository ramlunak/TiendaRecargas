using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Extensions;
using TiendaRecargas.Models.Enums;
using TiendaRecargas.Utiles;

namespace TiendaRecargas.Models
{
    public class Recarga
    {
        public int id { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        public TipoRecarga tipoRecarga { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        public string numero { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        public int idValorRecarga { get; set; }

        public decimal valor { get; set; }
        public decimal monto { get; set; }
        public string descripcion { get; set; }
        public int idCuenta { get; set; }
        public DateTime date { get; set; } = DateTime.Now.ToEasternStandardTime();
        public int? semana { get; set; } = CultureInfo.GetCultureInfo("es-ES").Calendar.GetWeekOfYear(DateTime.Now.ToUniversalTime(), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        public int? year { get; set; } = DateTime.Now.ToEasternStandardTime().Year;
        public Enums.RecargaStatus status { get; set; }
        public string TransactionId { get; set; }
        public string TransactionMsg { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionResultCode { get; set; }
        public bool activo { get; set; } = true;
        public bool simularErro { get; set; }
        [NotMapped]
        public string nauta { get; set; }

        public decimal GetMonto(decimal porciento)
        {
            return monto = valor / 100 * porciento;
        }
    }

    public class RecargaSearch
    {
        public string input { get; set; } = DateTime.Now.GetYearSemana();
        public int year
        {
            get
            {
                if (input is null) return DateTime.Now.ToEasternStandardTime().Year;
                return Convert.ToInt32(input.Split("-")[0]);
            }
        }
        public int semana
        {
            get
            {
                if (input is null) return 0;
                var numeroSemana = input.Split("-")[1].ToString().Replace("W","");
                return Convert.ToInt32(numeroSemana);
            }
        }
    }
}
