namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class GroupPageAccessElement : ConfigurationElement
    {
        private const int DefaultRoleId = 2;

        [ConfigurationProperty("roleid", DefaultValue = DefaultRoleId)]
        public int RoleId => (int)this["roleid"];

        [ConfigurationProperty("name")]
        public string Name => (string)this["name"];

        [ConfigurationProperty("PageAccesses")]
        [ConfigurationCollection(
            typeof(PageAccessCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PageAccessCollection PageAccesses => (PageAccessCollection)base["PageAccesses"];

        [ConfigurationProperty("DeniedPages")]
        [ConfigurationCollection(typeof(DeniedPageCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public DeniedPageCollection DeniedPages => (DeniedPageCollection)base["DeniedPages"];
    }
}
