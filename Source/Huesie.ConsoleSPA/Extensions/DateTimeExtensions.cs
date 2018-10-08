using System;

namespace Huesie.ConsoleSPA.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns just time HH24:mm:SS
        /// </summary>
        public static string ToStringHH24mmss(this DateTime dateTime, string separator = ":")
        {
            return dateTime.ToString($"HH{separator}mm{separator}ss");
        }
    }
}
