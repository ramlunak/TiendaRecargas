using Microsoft.EntityFrameworkCore;
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
  
    public class RecargaValor
    {
        //[Key]
        ////[DisplayName("ID")]
        public int id { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Display(Name = "Valor")]        
        public decimal valor { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Display(Name = "Tipo de Recarga")]          
        public TipoRecarga tipoRecarga { get; set; }
              
    }
}
