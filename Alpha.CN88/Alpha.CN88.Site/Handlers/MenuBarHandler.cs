namespace Alpha.CN88.Site.Handlers
{
    using MediatR;
    using Requests.Default;
    using Services;
    using ViewModels.Default;

    public class MenuBarHandler :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService;

        public MenuBarHandler(SiteMenuService siteMenuService)
        {
            this.siteMenuService = siteMenuService;
        }

        public MenuBarViewModel Handle(MenuBarQuery message)
        {
            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel, message.IsCookieLess);

            return new MenuBarViewModel(menuItems);
        }
    }
}