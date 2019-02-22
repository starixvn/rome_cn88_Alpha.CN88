namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using CN88.Models;

    public class GeneralAnnouncementViewModel
    {
        public GeneralAnnouncementViewModel(IList<AnnouncementModel> announcements)
        {
            Announcements = announcements;
        }

        public IList<AnnouncementModel> Announcements { get; }
    }
}