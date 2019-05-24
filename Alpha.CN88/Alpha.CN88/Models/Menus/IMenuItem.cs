namespace Alpha.CN88.Models.Menus
{
    using System.Collections.Generic;

    public interface IMenuItem
    {
        string Key { get; set; }

        string ResourceKey { get; set; }

        string DisplayName { get; }

        string Link { get; set; }

        string AppPath { get; set; }

        bool ByPassAuthorization { get; set; }

        bool IsBeta { get; set; }

        bool DisplayOnlyOnMobile { get; set; }

        bool HideOnMobile { get; set; }

        string MobileResourceKey { get; set; }

        string MobileDisplayName { get; }

        MenuStatus Status { get; }

        bool ShowSubMenus { get; set; }

        List<IMenuItem> SubMenus { get; set; }

        IEnumerable<IMenuItem> VisibileSubMenus { get; }

        IEnumerable<IMenuItem> VisibileSubMenusOnMobile { get; }
    }
}
