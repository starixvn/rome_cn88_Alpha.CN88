namespace CN88.Web.ViewModel
{
    using CN88.Web.Models;

    public class BrowserViewModel
    {
        public bool IsUnsupportedBrowser { get; set; }

        public string DownloadNewBrowserLink { get; set; }

        public UserLevel UserLevel { get; set; }
    }
}
