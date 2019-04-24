namespace Alpha.CN88
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Fanex.Logging.Extensions.Web;
    using Helpers;
    using Models.Menus;
    using Sunplus.Agent.Utilities;
    using static Sunplus.Agent.Model.Entities.UserSession;

    public static class FunctionRegistry
    {
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["TotalBetsMenuItem"] = () => new MenuStatus(Current.SubPageAccess.ViewTotalBets),
            ["AccountManagementMenuItem"] = () => new MenuStatus(Current.SubPageAccess.ViewMemberInfo || Current.SubPageAccess.UpdateAccount),
            ["ReportMenuItem"] = () => new MenuStatus(() => Current.SubPageAccess.ViewReports, null),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc)
        };

        public static readonly IDictionary<string, Func<string>> MenuItemDisplayNameRules = new Dictionary<string, Func<string>>
        {
        };

        private static Func<string> getDefaultUrl = () =>
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var defaultMainPage = HttpContext.Current.Response.ApplyAppPathModifier("~/Home/Index");
            var currentUrlCookieName = CookieHelper.GetCookieNameOfCurrentPage(Current.CustId, Current.SubAccId, Encryption.PearsonEncode);

            var currentURLCookie = HttpContext.Current.Request.Cookies[currentUrlCookieName];

            if (currentURLCookie != null &&
                !string.IsNullOrWhiteSpace(currentURLCookie.Value) &&
                !Current.IsInternal &&
                (currentURLCookie.Value.IndexOf("http", StringComparison.OrdinalIgnoreCase) >= 0 || currentURLCookie.Value.IndexOf("https", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                try
                {
                    Uri currentUrl = new Uri(HttpUtility.UrlDecode(currentURLCookie.Value));

                    if (HttpContext.Current.Request.Url.Host == currentUrl.Host)
                    {
                        return currentUrl.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex, TraceEventType.Information);
                }
            }

            return defaultMainPage;
        };

        public static Func<string> GetDefaultUrl
        {
            get
            {
                return getDefaultUrl;
            }
        }
    }
}
