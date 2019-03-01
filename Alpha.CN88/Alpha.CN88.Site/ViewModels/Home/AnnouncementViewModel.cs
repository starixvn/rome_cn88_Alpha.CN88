namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using CN88.Models;
    using Constants;
    using Web.Helpers;

    public class AnnouncementViewModel
    {
        public AnnouncementViewModel(IList<AnnouncementModel> announcements)
        {
            Announcements = announcements;
        }

        public IList<AnnouncementModel> Announcements { get; }

        public string ImportantAnnouncementPath
            => MicroSites.MessageUrl($"~/MessageHistory/Index?messageType={MessageType.Normal}");
    }
}