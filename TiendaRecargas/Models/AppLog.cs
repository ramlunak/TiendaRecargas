using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    [Table("App_Log")]
    public class AppLog
    {
        [Key]
        public int IdAppLog { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int IdCuenta { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? Created_at { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Exception { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string LogFrom { get; set; }

        //Relations
        public Cuenta Cuenta { get; set; }
    }
}
