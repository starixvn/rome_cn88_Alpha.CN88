namespace Alpha.CN88.Site.Handlers
{
    using global::CN88.Web.Helpers;
    using MediatR;
    using Requests.Default;
    using Services;
    using ViewModels.Default;

    public class MenuBarHandler :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService;
        private readonly BrowserHelper browserHelper;

        public MenuBarHandler(
            SiteMenuService siteMenuService,
            BrowserHelper browserHelper)
        {
            this.siteMenuService = siteMenuService;
            this.browserHelper = browserHelper;
        }

        public MenuBarViewModel Handle(MenuBarQuery message)
        {
            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel, message.IsCookieLess);

            return new MenuBarViewModel(menuItems, browserHelper.IsMobileTablet());
        }
    }
}