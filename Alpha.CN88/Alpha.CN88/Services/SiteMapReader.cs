namespace Alpha.CN88.Services
{
    using System.Web;

    public interface SiteMapReader
    {
        SiteMapNode GetRootNode(SiteMapNode rootNode = null);
    }
}
