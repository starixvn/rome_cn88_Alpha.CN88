namespace Alpha.CN88.Site.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Models.Home;
    using Configurations;
    using Helpers;
    using Requests.Home;
    using Web.Helpers;

    public class HomeViewModel
    {
        public HomeViewModel(HomeQuery query, AccountOverview overView, IEnumerable<StatusSummary> statusSummaries)
        {
            Message = query;
            OverView = overView;
            StatusSummaries = statusSummaries.OrderByDescending(i => i.RoleId);
            TotalStatus = GetTotal();
        }

        private HomeQuery Message { get; }

        public AccountOverview OverView { get; }

        public IEnumerable<StatusSummary> StatusSummaries { get; }

        public StatusSummary TotalStatus { get; }

        public bool ShowCustomerListUrl(int number, byte roleId)
            => number > 0 && Message.ActorLevel.GetDirectDownLineLevel().Value == roleId;

        public string CustomerListUrl(int number, int status, byte roleId)
        {
            var customerListUrl = string.Empty;
            if (ShowCustomerListUrl(number, roleId))
            {
                customerListUrl = UrlHelper.AddQueryStringParamsFunc(
                      AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/CustomerList/Index"),
                      new Dictionary<string, object>
                      {
                          ["SearchRoleId"] = roleId,
                          ["Status"] = status
                      });
            }

            return customerListUrl;
        }

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