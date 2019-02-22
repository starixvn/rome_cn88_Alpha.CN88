namespace Alpha.CN88.Site.Handlers
{
    using MediatR;
    using Requests.Home;
    using Services;
    using ViewModels.Home;

    public class HomeHandler :
        IRequestHandler<AccountOverviewQuery, AccountOverviewViewModel>
    {
        private readonly HomeService homeService;

        public HomeHandler(HomeService homeService)
        {
            this.homeService = homeService;
        }

        public AccountOverviewViewModel Handle(AccountOverviewQuery message)
        {
            var model = homeService.GetAccountOverview(message.CustId);

            return new AccountOverviewViewModel(model);
        }
    }
}