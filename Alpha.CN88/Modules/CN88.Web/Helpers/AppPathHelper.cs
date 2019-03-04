namespace CN88.Web.Helpers
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    internal static class AppPathHelper
    {
        internal static string GetAppPath(string appName, string virtualPath)
        {
            return GetAppPath(appName, virtualPath, null, null);
        }

        internal static string GetAppPath(
            string appName,
            string virtualPath,
            Func<HttpContext> getCurrentContext,
            Func<string> getAppDomainVirtualPath)
        {
            getCurrentContext = getCurrentContext ?? (() => { return HttpContext.Current; });
            getAppDomainVirtualPath = getAppDomainVirtualPath ?? (() => HttpRuntime.AppDomainAppVirtualPath);

            var context = getCurrentContext();

            if (context == null || context.CurrentHandler == null)
            {
                return string.Empty;
            }

            var appPath = context.Response.ApplyAppPathModifier(virtualPath);

            return Regex.Replace(appPath, getAppDomainVirtualPath(), appName, RegexOptions.IgnoreCase);
        }
    }
}
