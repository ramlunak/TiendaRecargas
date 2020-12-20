using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToTimeZone(this DateTime date, string id)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById(id);
            var data = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), kstZone);
            return data;
        }

        public static DateTime ToEasternStandardTime(this DateTime date)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var data = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), kstZone);
            return data;
        }
    }
}
