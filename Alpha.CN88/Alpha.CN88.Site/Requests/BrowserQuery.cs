namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;
    using global::CN88.Web.ViewModel;
    using MediatR;

    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        private readonly IUser user;

        public BrowserQuery(IUser user)
        {
            this.user = user;
        }

        public byte UserLevelId => user.LevelId;
    }
}