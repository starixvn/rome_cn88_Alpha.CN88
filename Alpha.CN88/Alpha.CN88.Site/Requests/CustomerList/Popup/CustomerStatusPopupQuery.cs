namespace Alpha.CN88.Site.Requests.CustomerList
{
    using CN88.Models;
    using MediatR;
    using ViewModels.CustomerList;

    public class CustomerStatusPopupQuery : BaseQuery, IRequest<StatusPopupViewModel>
    {
        public CustomerStatusPopupQuery(IUser viewer) : base(viewer)
        {
        }

        public int UpdateStatusId { get; set; }
    }
}