namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class GroupPageAccessCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
            => ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName
            => "GroupPageAccess";

        public new GroupPageAccessElement this[string path]
            => (GroupPageAccessElement)BaseGet(path);

        public GroupPageAccessElement this[int index]
            => (GroupPageAccessElement)BaseGet(index);

        public int IndexOf(GroupPageAccessElement authenticationElement)
            => BaseIndexOf(authenticationElement);

        protected override ConfigurationElement CreateNewElement()
            => new GroupPageAccessElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((GroupPageAccessElement)element).RoleId;
    }
}
