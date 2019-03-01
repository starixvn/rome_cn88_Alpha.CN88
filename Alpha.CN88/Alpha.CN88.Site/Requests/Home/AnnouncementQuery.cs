namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class AnnouncementQuery : BaseQuery, IRequest<AnnouncementViewModel>
    {
        public AnnouncementQuery(IUser viewer) : base(viewer)
        {
        }
    }
}