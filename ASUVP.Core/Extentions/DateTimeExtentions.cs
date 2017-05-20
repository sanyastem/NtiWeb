using System;
using ASUVP.Core.Configuration;

namespace ASUVP.Core.Extentions
{
    public static class DateTimeExtentions
    {
        public static string ToShort(this DateTime? date)
        {
            return date.HasValue ? ToShort(date.Value) : string.Empty;
        }

        public static string ToShort(this DateTime date)
        {
            return date.ToString(ConfigManager.ShortDateTimeFormat);
        }

        public static string ToLong(this DateTime? date)
        {
            return date.HasValue ? ToLong(date.Value) : string.Empty;
        }

        public static string ToLong(this DateTime date)
        {
            return date.ToString(ConfigManager.LongDateTimeFormat);
        }
    }
}