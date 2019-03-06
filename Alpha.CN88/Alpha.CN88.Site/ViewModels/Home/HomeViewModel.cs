namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using CN88.Models.Home;
    public class HomeViewModel
    {
        public HomeViewModel(AccountOverview overView, IEnumerable<StatusSummary> statusSummaries)
        {
            OverView = overView;
            StatusSummaries = statusSummaries;
            TotalStatus = GetTotal();
        }

        public AccountOverview OverView { get; }

        public IEnumerable<StatusSummary> StatusSummaries { get; }

        public StatusSummary TotalStatus { get; }

        private StatusSummary GetTotal()
        {
            var result = new StatusSummary();
            foreach (var s in StatusSummaries)
            {
                result.Add(s);
            }

            return result;
        }
    }
}