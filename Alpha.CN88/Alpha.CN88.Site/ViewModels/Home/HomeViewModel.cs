namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Models.Home;
    public class HomeViewModel
    {
        public HomeViewModel(AccountOverview overView, IEnumerable<StatusSummary> statusSummaries)
        {
            OverView = overView;
            StatusSummaries = statusSummaries.OrderByDescending(i => i.RoleId);
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