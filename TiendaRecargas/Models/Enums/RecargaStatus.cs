using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TiendaRecargas.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RecargaStatus : short
    {
        en_lista = 0,
        success = 1,
        error = 2,
    }
}
