namespace CN88.Common.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string DateWithHyphenPattern = "yyyy-MM-dd";
        private const string DateTimeWithHyphenPattern = "yyyy-MM-dd HH:mm:ss";

        public static string ToDateWithHyphen(this DateTime dateTime)
            => dateTime.ToString(DateWithHyphenPattern);

        public static string ToDateTimeWithHyphen(this DateTime dateTime)
            => dateTime.ToString(DateTimeWithHyphenPattern);
    }
}