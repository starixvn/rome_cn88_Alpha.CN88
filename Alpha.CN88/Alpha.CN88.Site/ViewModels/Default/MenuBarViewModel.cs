namespace Alpha.CN88.Site.ViewModels.Default
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Models.Menus;

    public class MenuBarViewModel
    {
        public MenuBarViewModel(
            IEnumerable<IMenuItem> menuItems,
            bool isMobile)
        {
            MenuItems = menuItems
                .Where(menuItem => menuItem.Status.Visible);
            IsMobile = isMobile;
        }

        public IEnumerable<IMenuItem> MenuItems { get; }

        public bool IsMobile { get; }

        public IEnumerable<IMenuItem> GetSubMenus(IMenuItem menuItem)
            => IsMobile ? menuItem.VisibileSubMenusOnMobile : menuItem.VisibileSubMenus; 

        public string GetDisplayName(IMenuItem menuItem)
            => IsMobile ? menuItem.MobileDisplayName : menuItem.DisplayName;
    }
}