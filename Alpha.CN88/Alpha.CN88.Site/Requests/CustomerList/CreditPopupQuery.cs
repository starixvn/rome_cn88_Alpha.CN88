namespace Alpha.CN88.Site.Requests.CustomerList
{
    using CN88.Models;
    using MediatR;
    using ViewModels.CustomerList;

    public class CreditPopupQuery : BaseQuery, IRequest<CreditPopupViewModel>
    {
        public CreditPopupQuery(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public decimal UsedCredit { get; set; }

        public decimal LimitedCredit { get; set; }
    }
}