namespace Alpha.CN88.Services
{
    using System.Collections.Generic;
    using global::CN88.Web.Models;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false);
    }
}
