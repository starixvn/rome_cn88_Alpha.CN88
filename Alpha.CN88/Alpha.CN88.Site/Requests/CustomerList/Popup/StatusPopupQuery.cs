namespace Alpha.CN88.Site.Requests.CustomerList
{
    using CN88.Models;
    using MediatR;
    using ViewModels.CustomerList;

    public class StatusPopupQuery : BaseQuery, IRequest<StatusPopupViewModel>
    {
        public StatusPopupQuery(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public int Status { get; set; }
    }
}