namespace Alpha.CN88.Site.ViewModels.Default
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Models.Menus;

    public class MenuBarViewModel
    {
        public MenuBarViewModel(IEnumerable<IMenuItem> menuItems)
        {
            MenuItems = menuItems.Where(menuItem => menuItem.Status.Visible);
        }

        public IEnumerable<IMenuItem> MenuItems { get; }
    }
}