namespace Alpha.CN88.Services
{
    using Models;

    public interface HomeService
    {
        AccountOverview GetAccountOverview(int custId);
    }
}
