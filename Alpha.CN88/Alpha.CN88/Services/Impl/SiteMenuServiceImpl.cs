namespace Alpha.CN88.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using global::CN88.Common.Models;
    using Models.Menus;

    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.CN88.MenuForLevel.";
        private readonly CachingManager cacheManager;
        private readonly SiteMapReader siteMapReader;
        private readonly MenuItemBuilder menuItemBuilder;

        public SiteMenuServiceImpl(
            CachingManager cacheManager,
            SiteMapReader siteMapReader,
            MenuItemBuilder menuItemBuilder)
        {
            this.cacheManager = cacheManager;
            this.siteMapReader = siteMapReader;
            this.menuItemBuilder = menuItemBuilder;
        }

        public IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false)
        {
            var menuItems = GetMenuDataByUserLevel(userLevel, isCookieLess);

            return menuItems;
        }

        private List<IMenuItem> GetMenuDataByUserLevel(UserLevel userLevel, bool isCookieLess)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;

            List<IMenuItem> menuItems = cacheManager.Get<List<IMenuItem>>(cacheKey);

            if (menuItems == null || isCookieLess)
            {
                var rootNode = siteMapReader.GetRootNode();

                menuItems = BuildMenu(rootNode, userLevel);

                if (!isCookieLess)
                {
                    cacheManager.Add(cacheKey, menuItems);
                }
            }

            return menuItems;
        }

        private List<IMenuItem> BuildMenu(SiteMapNode siteMapNode, UserLevel userLevel)
        {
            var menuItems = new List<IMenuItem>();

            if (siteMapNode.HasChildNodes)
            {
                siteMapNode.ChildNodes.Cast<SiteMapNode>().ToList().ForEach(
                    node =>
                    {
                        if (node.Roles.Count == 0
                            || node.Roles.Contains(userLevel.DisplayName)
                            || node.Roles.Contains(userLevel.DisplayName.ToLowerInvariant())
                            || node.Roles.Contains(userLevel.Value.ToString()))
                        {
                            var menuItem = menuItemBuilder.BuildMenuItem(node);

                            if (node.HasChildNodes)
                            {
                                menuItem.SubMenus = BuildMenu(node, userLevel);
                            }

                            menuItems.Add(menuItem);
                        }
                    });
            }

            return menuItems;
        }
    }
}
