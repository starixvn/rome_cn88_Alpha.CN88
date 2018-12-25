namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using CN88.Models;
    using Requests.CustomerList;
    using Web.Helpers;

    public class IndexViewModel
    {
        public IndexViewModel(IndexQuery query)
        {
            ActorLevel = query.ActorLevel;
            IsInternal = query.IsInternal;
            HasUpdatePermission = query.HasUpdatePermission;
            HasAddNewPermission = query.HasAddNewPermission;

            Language = query.Language;
            ViewerId = query.ViewerId;
        }

        public UserLevel ActorLevel { get; set; }

        public bool IsInternal { get; set; }

        public bool HasUpdatePermission { get; set; }

        public bool HasAddNewPermission { get; set; }

        public static string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public static string AddCustomerLink => MicroSites.CN88Url("~/CN88/CreateCustomer");

        public static string EditCustomerLink => MicroSites.CN88Url("~/CN88/ViewCustomerSetting/Index");

        public string Language { get; }

        public int ViewerId { get; set; }
    }
}