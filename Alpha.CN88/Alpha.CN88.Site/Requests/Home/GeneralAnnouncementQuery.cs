namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class GeneralAnnouncementQuery : BaseQuery, IRequest<GeneralAnnouncementViewModel>
    {
        public GeneralAnnouncementQuery(IUser viewer) : base(viewer)
        {
        }
    }
}