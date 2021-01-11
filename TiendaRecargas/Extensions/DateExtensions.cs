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
        public static int GetSemana(this DateTime date)
        {
            return CultureInfo.GetCultureInfo("es-ES").Calendar.GetWeekOfYear(date.ToEasternStandardTime(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        public static string GetYearSemana(this DateTime date)
        {
            return $"{date.ToEasternStandardTime().Year}-W{CultureInfo.GetCultureInfo("es-ES").Calendar.GetWeekOfYear(date.ToEasternStandardTime(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday)}";
        }

        public static DateTime FirstDateOfWeek(this string s)
        {
            var array = s.Split('-');
            var year = Convert.ToInt32(array[0]);
            var semana = array[1];
            var weekOfYear = Convert.ToInt32(semana.Replace("W",""));
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (int)CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.CalendarWeekRule, CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }
    }
}
