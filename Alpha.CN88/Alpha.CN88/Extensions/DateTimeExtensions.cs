namespace Alpha.CN88.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "yyyy-MM-dd";

        public static string ToShortDateFormat(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }
    }
}
