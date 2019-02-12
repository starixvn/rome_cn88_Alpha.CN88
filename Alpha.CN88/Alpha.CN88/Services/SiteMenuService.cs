namespace Alpha.CN88.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false);
    }
}
