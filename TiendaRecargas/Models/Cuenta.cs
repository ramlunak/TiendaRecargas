using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiendaRecargas.Utiles;

namespace TiendaRecargas.Models
{
    public class Cuenta
    {
        [Key]
        [DisplayName("ID")]
        public int IdCuenta { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string Usuario { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(max)")]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = AppMessages.Required)]
        [Compare("Password", ErrorMessage = "La contraseña no coincide.")]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(30)")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(max)")]
        [DataType(DataType.EmailAddress, ErrorMessage = AppMessages.Email)]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Rol { get; set; }

        [Column(TypeName = "int")]
        public int? IdCuentaPadre { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency, ErrorMessage = AppMessages.Money)]
        [DisplayName("Crédito")]
        public decimal Credito { get; set; }

        public decimal CreditoBloqueado { get; set; } = 0;

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency, ErrorMessage = AppMessages.Money)]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        public decimal Porciento { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "bit")]
        [DisplayName("Tienda Física")]
        public bool TiendaFisica { get; set; } = false;

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "bit")]
        [DisplayName("Enviar SMS")]
        public bool EnviarSMS { get; set; } = false;

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "bit")]
        public bool Activo { get; set; } = true;

        //PARA CALCULAR % 

        [DisplayName("Precio de la recarga")]
        [NotMapped]
        public decimal PrecioRecarga { get; set; }
        [NotMapped]
        public decimal Fondos
        {
            get
            {
                return this.Credito - Balance - this.CreditoBloqueado;
            }
        }

        //para facturacion
        [NotMapped]
        public List<Recarga> Recargas { get; set; } = new List<Recarga>();
    }

    public class CuentaUpdatePassword
    {
        public int idCuenta { get; set; }
        public string Usuario { get; set; }

        [Required(ErrorMessage = AppMessages.Required)]
        [Column(TypeName = "nvarchar(max)")]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = AppMessages.Required)]
        [Compare("Password", ErrorMessage = "La contraseña no coincide.")]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}
