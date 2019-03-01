namespace Alpha.CN88.Site.Handlers
{
    using CN88.Models.Home;
    using MediatR;
    using Requests.Home;
    using Services;
    using ViewModels.Home;

    public class HomeHandler :
        IRequestHandler<AccountOverviewQuery, AccountOverviewViewModel>,
        IRequestHandler<StatusSummaryQuery, StatusSummaryReport>,
        IRequestHandler<AnnouncementQuery, AnnouncementViewModel>
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

        public StatusSummaryReport Handle(StatusSummaryQuery message)
        {
            var model = homeService.GetStatusSummary(message.CustId, message.ActorLevel);

            return model;
        }

        public AnnouncementViewModel Handle(AnnouncementQuery message)
        {
            var model = homeService.GetImportantAnnouncement(message.CustId, message.Language);

            return new AnnouncementViewModel(model);
        }
    }
}