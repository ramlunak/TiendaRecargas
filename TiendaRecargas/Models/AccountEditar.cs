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
    public class AccountEditar
    {
        [Required(ErrorMessage = AppMessages.Required)]
        [StringLength(255)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = AppMessages.Required)]
        [StringLength(255)]
        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [StringLength(255)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [StringLength(255)]
        [DisplayName("Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = AppMessages.Required)]
        //[Column(TypeName = "nvarchar(max)")]
        //[DisplayName("Contraseña")]
        //public string Password { get; set; }

        //[NotMapped]
        //[Required(ErrorMessage = AppMessages.Required)]
        //[Compare("Password", ErrorMessage = "La contraseña no coincide.")]
        //[DisplayName("Confirmar Contraseña")]
        //public string ConfirmPassword { get; set; }


    }
}
