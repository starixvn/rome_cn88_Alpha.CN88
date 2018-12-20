namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using Alpha.CN88.Site.Requests.CustomerList;

    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(StatusPopupQuery query)
        {
            Status = query.Status;
        }

        public int Status { get; }
    }
}