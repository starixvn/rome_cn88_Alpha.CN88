namespace Alpha.CN88.Services
{
    using Models;
    using Models.Home;

    public interface HomeService
    {
        AccountOverview GetAccountOverview(int custId);

        StatusSummaryReport GetStatusSummary(int custId, UserLevel userLevel);
    }
}
