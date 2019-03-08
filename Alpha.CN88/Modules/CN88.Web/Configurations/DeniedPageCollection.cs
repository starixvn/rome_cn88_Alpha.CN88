namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class DeniedPageCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
            => ConfigurationElementCollectionType.AddRemoveClearMap;

        public DeniedPageElement this[int index]
            => (DeniedPageElement)BaseGet(index);

        public new DeniedPageElement this[string name]
            => (DeniedPageElement)BaseGet(name);

        public void Add(DeniedPageElement authenticationLevelElement)
            => BaseAdd(authenticationLevelElement);

        public void Remove(DeniedPageElement authenticationLevelElement)
        {
            if (BaseIndexOf(authenticationLevelElement) >= 0)
            {
                BaseRemove(authenticationLevelElement.Url);
            }
        }

        public void RemoveAt(int index)
            => BaseRemoveAt(index);

        public void Remove(string alias)
            => BaseRemove(alias);

        public void Clear()
            => BaseClear();

        public int IndexOf(DeniedPageElement authenticationLevelElement)
            => BaseIndexOf(authenticationLevelElement);

        protected override sealed ConfigurationElement CreateNewElement()
            => new DeniedPageElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((DeniedPageElement)element).Url;

        protected override void BaseAdd(ConfigurationElement element)
            => BaseAdd(element, false);
    }
}
