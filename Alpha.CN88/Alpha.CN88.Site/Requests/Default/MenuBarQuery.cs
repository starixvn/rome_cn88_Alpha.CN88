namespace Alpha.CN88.Site.Requests.Default
{
    using System.Web;
    using global::CN88.Common.Models;
    using MediatR;
    using ViewModels.Default;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        public MenuBarQuery(byte roleId, string userAgent)
        {
            UserLevel = Enumeration.FromValue<UserLevel>(roleId);
            UserAgent = userAgent;
        }

        public UserLevel UserLevel { get; }

        public string UserAgent { get; }

        public bool IsCookieLess => HttpContext.Current != null && HttpContext.Current.Session.IsCookieless;
    }
}
