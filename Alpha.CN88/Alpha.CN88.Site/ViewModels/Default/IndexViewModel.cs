namespace Alpha.CN88.Site.ViewModels.Default
{
    using System;
    using CN88.Helpers;
    using Configurations;
    using Helpers;
    using Requests.Default;
    using Web.Helpers;

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
            CustName = message.CustName;
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
        }

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLower();
                var userAgent = browserHelper.GetUserAgent().ToLower();
                var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                var isSmallScale = browserName.Contains("chrome") &&
                    userAgent.Contains("android") &&
                    !browserHelper.IsAndroidNativeBrowser(userAgent) &&
                    !isTablet;

                return isSmallScale ? 0.5 : 0.7;
            }
        }

        public long OnlineMember { get; }

        public string CustName { get; }

        public int CustId { get; }

        public int RoleId { get; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public int OnlineUserId { get; }

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);

        public string PersonalMessagePath => MicroSites.MessageUrl("~/MessageHistory/Index?messageType=0");

        public string OnlineMembersPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/CustomerList/OnlineMembers");

        public string OldReportsPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/OldReports/VerifyPassword");

        public bool ClearClientData { get; }
    }
}