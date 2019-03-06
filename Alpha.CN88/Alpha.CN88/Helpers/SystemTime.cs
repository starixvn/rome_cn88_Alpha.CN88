using System;

namespace Alpha.CN88.Helpers
{
    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;

        public static DateTime Today
        {
            get { return today ?? DateTime.Today; }
            internal set { today = value; }
        }
    }
}
