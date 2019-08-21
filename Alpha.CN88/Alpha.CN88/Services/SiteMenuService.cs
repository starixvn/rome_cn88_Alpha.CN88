namespace Alpha.CN88.Services
{
    using System.Collections.Generic;
    using global::CN88.Common.Models;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false);
    }
}
