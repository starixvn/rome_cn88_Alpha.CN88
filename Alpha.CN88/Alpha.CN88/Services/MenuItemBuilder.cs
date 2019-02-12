namespace Alpha.CN88.Services
{
    using System.Web;
    using Models.Menus;

    public interface MenuItemBuilder
    {
        IMenuItem BuildMenuItem(SiteMapNode node);
    }
}
