namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using CN88.Models;
    using Web.Helpers;

    public class GeneralAnnouncementViewModel
    {
        public GeneralAnnouncementViewModel(IList<AnnouncementModel> announcements)
        {
            Announcements = announcements;
        }

        public IList<AnnouncementModel> Announcements { get; }

        public string GeneralAnnouncementPath => MicroSites.MessageUrl("~/MessageHistory/Index?messageType=0");
    }
}