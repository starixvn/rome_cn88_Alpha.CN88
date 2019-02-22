namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class ImportantAnnouncementQuery : BaseQuery, IRequest<ImportantAnnouncementViewModel>
    {
        public ImportantAnnouncementQuery(IUser viewer) : base(viewer)
        {
        }
    }
}