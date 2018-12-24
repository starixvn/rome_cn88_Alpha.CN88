namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using Alpha.CN88.Site.Requests.CustomerList;

    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(CustomerStatusPopupQuery query)
        {
            CustId = query.CustId;
            UpdateStatusId = query.UpdateStatusId;
        }

        public int CustId { get; }

        public int UpdateStatusId { get; }
    }
}