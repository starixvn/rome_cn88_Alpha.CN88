namespace Alpha.CN88.DataAccesses
{
    using Criteria.Home;
    using Models;

    public static class HomeDataAccess
    {
        public static AccountOverview GetAccountOverview(GetGetAccountOverviewCriteria criteria)
        {
            return new AccountOverview(1, 1, 0, new System.DateTime(2019, 2, 22), new System.DateTime(2019, 2, 22));
        }
    }
}
