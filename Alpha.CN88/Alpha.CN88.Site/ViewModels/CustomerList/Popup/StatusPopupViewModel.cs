namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using Alpha.CN88.Site.Requests.CustomerList;

    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(CustomerStatusPopupQuery query)
        {
            UpdateStatusId = query.UpdateStatusId;
        }

        public int UpdateStatusId { get; }
    }
}