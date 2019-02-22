namespace Alpha.CN88.Services.Impl
{
    using Criteria.Home;
    using DataAccesses;
    using Models;

    public class HomeServiceImpl : HomeService
    {
        public AccountOverview GetAccountOverview(int custId)
        {
            var criteria = new GetGetAccountOverviewCriteria(custId);

            return HomeDataAccess.GetAccountOverview(criteria);
        }
    }
}
