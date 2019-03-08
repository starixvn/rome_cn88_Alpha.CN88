namespace CN88.Web.Helpers
{
    using Configurations;

    internal static class MicroSites
    {
        public static string MainUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.MainAppPath, virtualPath);
    }
}
