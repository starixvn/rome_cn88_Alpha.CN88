namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class AnnouncementsQuery : BaseQuery, IRequest<AnnouncementsViewModel>
    {
        public AnnouncementsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}