using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Utiles;

namespace TiendaRecargas.Models
{
    public class EditarBalance
    {

        [DisplayName("Nombre")]
        public string fullname { get; set; }

        [DisplayName("Balance")]
        public decimal balance { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        //[Range(0, 20000, ErrorMessage = "Valor no permitido")]
        public decimal editar_balance { get; set; }

        //[Required(ErrorMessage = AppMessages.Required)]
        //[DisplayName("Opción")]
        //public bool? adicionar { get; set; } = null;

    }
}
