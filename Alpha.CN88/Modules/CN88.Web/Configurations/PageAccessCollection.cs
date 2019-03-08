namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class PageAccessCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
            => ConfigurationElementCollectionType.AddRemoveClearMap;

        public PageAccessElement this[int index]
            => (PageAccessElement)BaseGet(index);

        public new PageAccessElement this[string name]
            => (PageAccessElement)BaseGet(name);

        public void Add(PageAccessElement authenticationLevelElement)
            => BaseAdd(authenticationLevelElement);

        public void Remove(PageAccessElement authenticationLevelElement)
        {
            if (BaseIndexOf(authenticationLevelElement) >= 0)
            {
                BaseRemove(authenticationLevelElement.Id);
            }
        }

        public void RemoveAt(int index)
            => BaseRemoveAt(index);

        public void Remove(string alias)
            => BaseRemove(alias);

        public void Clear()
            => BaseClear();

        public int IndexOf(PageAccessElement authenticationLevelElement)
            => BaseIndexOf(authenticationLevelElement);

        protected override sealed ConfigurationElement CreateNewElement()
            => new PageAccessElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((PageAccessElement)element).Id;

        protected override void BaseAdd(ConfigurationElement element)
            => BaseAdd(element, false);
    }
}
