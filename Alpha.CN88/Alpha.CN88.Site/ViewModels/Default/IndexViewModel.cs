namespace Alpha.CN88.Site.ViewModels.Default
{
    using System;
    using System.Web;
    using CN88.Helpers;
    using global::CN88.Web.Helpers;
    using Helpers;
    using Requests.Default;

    public class IndexViewModel
    {
        private readonly BrowserHelper browserHelper;
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;

        public IndexViewModel(
            IndexQuery message,
            BrowserHelper browserHelper,
            long onlineMember,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction)
        {
            CustName = message.SubAccountId == 0 ? message.ViewerName : message.SubUserName;
            CustId = message.CustId;
            SubAccId = message.SubAccountId;
            OnlineUserId = message.OnlineUserId;
            IsInternal = message.IsInternal;
            RoleId = message.CustLevelId;
            ClearClientData = message.ClearClientData;

            this.browserHelper = browserHelper;
            OnlineMember = onlineMember;
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction;
            IsNotHeadline = message.ActorLevel.IsNotHeadlineLevel();
            CurrentLanguage = message.Language;
            NickName = message.NickName;
            AdminName = message.AdminName;
            ShowCN88SaveButton = message.ShowCN88SaveButton;
            ShowProblemAccountIcon = message.ShowProblemAccountIcon;
            IsHideTopMenu = message.IsHideTopMenu;
        }

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLowerInvariant();
                var userAgent = browserHelper.GetUserAgent().ToLowerInvariant();
                var isTablet = browserHelper.IsTablet();

                var isSmallScale = browserName.Contains("chrome") &&
                    userAgent.Contains("android") &&
                    !browserHelper.IsAndroidNativeBrowser(userAgent) &&
                    !isTablet;

                return isSmallScale ? 0.5 : 0.8;
            }
        }

        public long OnlineMember { get; }

        public string CustName { get; }

        public int CustId { get; }

        public int RoleId { get; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public int OnlineUserId { get; }

        public bool IsNotHeadline { get; }

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);

        public string PersonalMessagePath => MicroSites.MessageUrl("~/MessageHistory/Index?messageType=1");

        public string ProblemAccountPath => MicroSites.AccountsUrl("~/ProblemAccount/Index");

        public string OnlineMembersPath => MicroSites.FeaturesUrl("~/CustomerList/OnlineMembers");

        public string OldReportsPath => MicroSites.FeaturesUrl("~/OldReports/Index?DisplayMode=1");

        public string WagerMonitoringConsolePath => MicroSites.FeaturesUrl("~/WagerMonitoring/Index");

        public bool ClearClientData { get; }

        public bool IsDevice => browserHelper.IsMobile();

        public string TroubleshootingPath => UrlHelper.MapMainPath("~/Help/Troubleshooting");

        public string CurrentLanguage { get; }

        public bool IsSelectedLanguage(string code) => string.Equals(code, CurrentLanguage, StringComparison.OrdinalIgnoreCase);

        public bool IsShowMySettingPath
            => SubAccId == 0 && IsNotHeadline;

        public string MySettingPath
            => IsShowMySettingPath
            ? MicroSites.FeaturesUrl($"~/CN88CustomerSetting/ViewCN88CustomerSetting/MySetting?customerLevel={RoleId}&isCreateNew=false&productId=1")
            : string.Empty;

        public string NickName { get; }

        public string AdminName { get; }

        public string InitMainFrameLink
        {
            get
            {
                var result = HttpContext.Current.Request.Cookies.Get(CurrentURLCookieName)?.Value;

                if (result == null)
                {
                    return string.Empty;
                }

                var cookie = HttpContext.Current.Request.Cookies[CurrentURLCookieName];
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);

                return result;
            }
        }

        public bool ShowCN88SaveButton { get; }

        public bool ShowProblemAccountIcon { get; }

        public bool IsHideTopMenu { get; }
    }
}
