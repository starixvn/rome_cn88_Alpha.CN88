namespace Alpha.CN88.Services.Impl
{
    using System.Collections.Generic;
    using Criteria.Home;
    using DataAccesses;
    using Models;
    using Models.Home;

    public class HomeServiceImpl : HomeService
    {
        public AccountOverview GetAccountOverview(int custId)
        {
            var criteria = new GetAccountOverviewCriteria(custId);

            return HomeDataAccess.GetAccountOverview(criteria);
        }

        public StatusSummaryReport GetStatusSummary(int custId, UserLevel userLevel)
        {
            var criteria = new GetStatusSummaryCriteria(custId, userLevel);

            return HomeDataAccess.GetStatusSummary(criteria);
        }

        public IList<AnnouncementModel> GetAnnouncements(int custId, string language)
        {
            var criteria = new GetImportantAnnouncementCriteria(custId, language);

            return HomeDataAccess.GetImportantAnnouncement(criteria);
        }
    }
}
