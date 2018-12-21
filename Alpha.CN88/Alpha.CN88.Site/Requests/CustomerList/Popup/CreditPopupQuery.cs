namespace Alpha.CN88.Site.Requests.CustomerList
{
    using Alpha.CN88.Site.ViewModels.CustomerList;
    using CN88.Models;
    using MediatR;

    public class CreditPopupQuery : BaseQuery, IRequest<CreditRangeViewModel>
    {
        public CreditPopupQuery(IUser viewer) : base(viewer)
        {
        }
    }
}