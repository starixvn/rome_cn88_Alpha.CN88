namespace Alpha.CN88.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using AppResources;
    using Configurations;

    public class MenuItem : IMenuItem
    {
        private string link;
        private readonly Func<MenuStatus> getMenuStatusFunc;
        private readonly Predicate<string> authorizeUrlFunc;
        private readonly Func<string> getDisplayName;

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null)
            : this(getMenuStatusFunc, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc)
            : this(getMenuStatusFunc, authorizeUrlFunc, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc, Func<string> getDisplayName)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;
            this.getDisplayName = getDisplayName;
        }

        public string Key { get; set; }

        public string ResourceKey { get; set; }

        public string AppPath { get; set; }

        public bool ByPassAuthorization { get; set; }

        public bool DisplayOnlyOnMobile { get; set; }

        public bool HideOnMobile { get; set; }

        public bool IsBeta { get; set; }

        public string DisplayName
            => getDisplayName != null ? getDisplayName() : SitemapLabels.ResourceManager.GetString(ResourceKey);

        public virtual string Link
        {
            get
            {
                var appVirtualPath = AppSettings.GetAppPath(AppPath);

                if (!string.IsNullOrWhiteSpace(appVirtualPath))
                {
                    return Regex.Replace(link, HttpRuntime.AppDomainAppVirtualPath, appVirtualPath);
                }

                return link;
            }

            set
            {
                link = value;
            }
        }

        public virtual MenuStatus Status
        {
            get
            {
                var isAuthorized = ByPassAuthorization || authorizeUrlFunc == null || authorizeUrlFunc(Link);

                return isAuthorized
                    ? getMenuStatusFunc()
                    : new MenuStatus(false, false);
            }
        }

        public bool ShowSubMenus { get; set; }

        public List<IMenuItem> SubMenus { get; set; }

        public IEnumerable<IMenuItem> VisibileSubMenus
            => SubMenus.Where(menuItem => menuItem.Status.Visible && !menuItem.DisplayOnlyOnMobile);

        public IEnumerable<IMenuItem> VisibileSubMenusOnMobile
            => SubMenus.Where(menuItem => menuItem.Status.Visible && !menuItem.HideOnMobile);

        public string MobileResourceKey { get; set; }

        public string MobileDisplayName
            => MobileResourceKey == null
                    ? DisplayName
                    : SitemapLabels.ResourceManager.GetString(MobileResourceKey);
    }
}
