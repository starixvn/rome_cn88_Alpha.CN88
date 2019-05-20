namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class DeniedPageElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Url => (string)this["url"];
    }
}
