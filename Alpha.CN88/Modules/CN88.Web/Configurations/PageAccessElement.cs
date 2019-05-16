namespace CN88.Web.Configurations
{
    using System.Configuration;

    public class PageAccessElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id => (string)this["id"];

        [ConfigurationProperty("permission", DefaultValue = "", IsRequired = true)]
        public string Permission => (string)this["permission"];

        [ConfigurationProperty("mainfuncname")]
        public string MainFunctionName => (string)this["mainfuncname"];

        [ConfigurationProperty("subfuncname")]
        public string SubFunctionName => (string)this["subfuncname"];

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true)]
        public string Url => (string)this["url"];

        [ConfigurationProperty("ischeckbelonging", DefaultValue = false)]
        public bool IsCheckBelonging => (bool)this["ischeckbelonging"];

        [ConfigurationProperty("isallowtodobythemselves", DefaultValue = false)]
        public bool IsAllowToDoByThemselves => (bool)this["isallowtodobythemselves"];

        [ConfigurationProperty("isviewedbygroupheadline", DefaultValue = false)]
        public bool IsViewedByGroupHeadline => (bool)this["isviewedbygroupheadline"];
    }
}
