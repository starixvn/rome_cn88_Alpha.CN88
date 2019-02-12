namespace Alpha.CN88.Site.Helpers
{
    using System;
    using System.Web;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapMainPath;

        public static Func<string, string> MapMainPath
        {
            get
            {
                return mapMainPath ?? HttpContext.Current.Response.ApplyAppPathModifier;
            }

            internal set
            {
                mapMainPath = value;
            }
        }
    }
}