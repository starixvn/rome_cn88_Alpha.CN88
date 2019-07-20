namespace Alpha.CN88.Site.Handlers
{
    using System.Linq;
    using MediatR;
    using Requests.Home;
    using Services;
    using ViewModels.Home;

    public class HomeHandler : IRequestHandler<HomeQuery, HomeViewModel>
    {
        private readonly HomeService homeService;

        public HomeHandler(HomeService homeService)
        {
            this.homeService = homeService;
        }

        public HomeViewModel Handle(HomeQuery message)
        {
            var model = homeService.GetGeneralInformation(
                message.CustId,
                message.SubAccountId,
                message.IsAllDownline);

            return new HomeViewModel(message, model.Item1.FirstOrDefault(), model.Item2);
        }
    }
}