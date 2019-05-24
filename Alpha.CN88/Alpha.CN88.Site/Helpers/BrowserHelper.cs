namespace Alpha.CN88.Site.Helpers
{
    using CN88.Models;
    using ViewModels;

    public interface BrowserHelper
    {
        bool IsMobile(bool isOnlyTablet);

        BrowserViewModel CheckSupportedBrowsers();

        Device GetDevice();

        string GetDeviceId();

        string GetUserAgent();

        bool IsAndroidNativeBrowser(string userAgent);

        BrowserInfo GetBrowserInfo();
    }
}