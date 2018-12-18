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
        }

        public UserLevel ActorLevel { get; set; }

        public bool IsInternal { get; set; }

        public static string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");
    }
}