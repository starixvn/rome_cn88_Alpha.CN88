namespace CN88.Web.Helpers
{
    using Configurations;

    public static class MicroSites
    {
        public static string MainUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.MainAppPath, virtualPath);
    }
}
