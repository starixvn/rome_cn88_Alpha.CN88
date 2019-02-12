namespace Alpha.CN88.Site.ViewModels.Default
{
    using System;
    using CN88.Helpers;
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
            CustName = message.CustName;
            CustId = message.CustId;
            SubAccId = message.SubAccountId;

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

        public int SubAccId { get; }

        public string MainRootPath => UrlHelper.MapMainPath("~/Home/Index");

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, SubAccId, encodeDataFunction);
    }
}