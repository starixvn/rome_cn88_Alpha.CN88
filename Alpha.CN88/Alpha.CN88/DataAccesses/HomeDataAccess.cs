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
    }
}
