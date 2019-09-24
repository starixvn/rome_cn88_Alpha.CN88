namespace CN88.Web.Helpers
{
    using Configurations;

    public static class MicroSites
    {
        public static string MainUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.MainAppPath, virtualPath);

        public static string MessageUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.MessageAppPath, virtualPath);

        public static string FeaturesUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, virtualPath);

        public static string AccountsUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.AccountAppPath, virtualPath);
    }
}
