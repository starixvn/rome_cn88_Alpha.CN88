namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class AccountOverviewQuery : BaseQuery, IRequest<AccountOverviewViewModel>
    {
        public AccountOverviewQuery(IUser viewer) : base(viewer)
        {
        }
    }
}