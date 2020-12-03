using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
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
        public DateTime date { get; set; } = DateTime.Now;
        public Enums.RecargaStatus status { get; set; }
        public string TransactionId { get; set; }
        public string TransactionMsg { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
