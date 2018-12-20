namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using Alpha.CN88.Site.Requests.CustomerList;

    public class CreditPopupViewModel
    {
        public CreditPopupViewModel(CreditPopupQuery query)
        {
            UsedCredit = query.UsedCredit;
            LimitedCredit = query.LimitedCredit;
        }

        public decimal UsedCredit { get; }

        public decimal LimitedCredit { get; }
    }
}