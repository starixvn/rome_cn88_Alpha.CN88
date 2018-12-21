namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using Alpha.CN88.Models;
    using Alpha.CN88.Models.CustomerList;
    using Alpha.CN88.Site.Requests.CustomerList;

    public class CreditRangeViewModel
    {
        public CreditRangeViewModel(CreditPopupQuery query, CreditRangeModel creditModel)
        {
            CreditModel = creditModel;
            CustLevel = query.CustLevel;
            CustId = query.CustId;
        }

        public CreditRangeModel CreditModel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }
    }
}