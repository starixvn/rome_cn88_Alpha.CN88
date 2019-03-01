namespace Alpha.CN88.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.Home;
    using Models;
    using Models.Home;

    public static class HomeDataAccess
    {
        public static AccountOverview GetAccountOverview(GetAccountOverviewCriteria criteria)
        {
            return new AccountOverview(1, 1, 0, new System.DateTime(2019, 2, 22), new System.DateTime(2019, 2, 22));
        }

        public static StatusSummaryReport GetStatusSummary(GetStatusSummaryCriteria criteria)
        {
            var list = new List<StatusSummary>
            {
                new StatusSummary("Agent", 0, 0, 0, 0),
                new StatusSummary("Member", 0, 0, 0, 0),
            };

            return new StatusSummaryReport(list);
        }

        public static IList<AnnouncementModel> GetImportantAnnouncement(GetImportantAnnouncementCriteria criteria)
        {
            var list = new List<AnnouncementModel>
            {
                new AnnouncementModel(new System.DateTime(2019, 2, 25), "Announcement 1"),
                new AnnouncementModel(new System.DateTime(2019, 5, 31), "Announcement 2"),
                new AnnouncementModel(new System.DateTime(2019, 5, 23), "Announcement 3"),
                new AnnouncementModel(new System.DateTime(2019, 5, 28), "Announcement 4"),
                new AnnouncementModel(new System.DateTime(2019, 3, 28), "Announcement 5"),
            };

            return list;
        }
    }
}
