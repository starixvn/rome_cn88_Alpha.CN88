namespace Alpha.CN88.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Fanex.Common.Helper;
    using global::CN88.Web.Services;
    using Models.Menus;

    public class MenuItemBuilderImpl : MenuItemBuilder
    {
        private readonly AuthorizationService authorizationService;
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly IDictionary<string, Func<string>> menuItemDisplayName;

        public MenuItemBuilderImpl() : this(null, null, null)
        {
        }

        internal MenuItemBuilderImpl(
            AuthorizationService authorizationService,
            IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration,
            IDictionary<string, Func<string>> menuItemDisplayName)
        {
            this.authorizationService = authorizationService ?? new AuthorizationServiceImpl();
            this.menuItemTypeConfiguration = menuItemTypeConfiguration ?? FunctionRegistry.MenuItemTypeStatusRules;
            this.menuItemDisplayName = menuItemDisplayName ?? FunctionRegistry.MenuItemDisplayNameRules;
        }

        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, nameof(node));

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node["menuItemType"] ?? string.Empty],
                authorizationService.IsAuthorized,
                menuItemDisplayName.ContainsKey(node.ResourceKey) ? menuItemDisplayName[node.ResourceKey] : null);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }

        private static void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.Key = node.Key;
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Link = node.Url;
            menuItem.AppPath = node["AppPath"];
            menuItem.IsBeta = node["isBeta"] != null && bool.Parse(node["isBeta"]);
            menuItem.HideOnMobile = node["hideOnMobile"] != null && bool.Parse(node["hideOnMobile"]);
            menuItem.MobileResourceKey = node["mobileResourceKey"];
            menuItem.DisplayOnlyOnMobile = node["displayOnlyOnMobile"] != null && bool.Parse(node["displayOnlyOnMobile"]);
            menuItem.ByPassAuthorization = node["byPassAuthorization"] != null && bool.Parse(node["byPassAuthorization"]);
            menuItem.SubMenus = new List<IMenuItem>();
        }
    }
}
