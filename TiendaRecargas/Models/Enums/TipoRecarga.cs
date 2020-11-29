using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TiendaRecargas.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TipoRecarga : short
    {
        [Display(Name = "Movil")]
        movil = 1,
        [Display(Name = "Nauta")]
        nauta = 2
    }
}
