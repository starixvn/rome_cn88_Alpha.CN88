namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using CN88.Models.Home;
    using MediatR;

    public class StatusSummaryQuery : BaseQuery, IRequest<StatusSummaryReport>
    {
        public StatusSummaryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}