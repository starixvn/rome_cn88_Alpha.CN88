namespace CN88.Web.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class AuthorizationSection : ConfigurationSection
    {
        private const int DefaultCachedExpireTime = 30;

        private static AuthorizationSection instance
           = ConfigurationManager.GetSection("AuthorizationSection") as AuthorizationSection;

        private static IEnumerable<GroupPageAccessElement> groupPageAccesses
            = instance?.GroupPageAccesses.Cast<GroupPageAccessElement>();

        private static string[] custIdQueryStringParamNames;
        private static string[] custIdJsonParamNames;

        public static AuthorizationSection Instance
            => instance;

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public GroupPageAccessCollection GroupPageAccesses
            => (GroupPageAccessCollection)base[string.Empty];

        public IEnumerable<GroupPageAccessElement> GroupPageAccessList
            => groupPageAccesses;

        [ConfigurationProperty("fullpermission", DefaultValue = "[A],[B],[C],[D],[E],[G],[H]")]
        public string FullPermission
            => (string)this["fullpermission"];

        [ConfigurationProperty(
            "custIdQueryStringParamNameMapping",
            DefaultValue = "custid,customerid")]
        public string CustIdQueryStringParamNameMapping
            => (string)this["custIdQueryStringParamNameMapping"];

        public string[] CustIdQueryStringParamNames
        {
            get
            {
                custIdQueryStringParamNames = custIdQueryStringParamNames
                    ?? CustIdQueryStringParamNameMapping.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return custIdQueryStringParamNames;
            }
        }

        [ConfigurationProperty("custIdJsonParamNameMapping", DefaultValue = "txtCustomerObject,newCustomer")]
        public string CustIdJsonParamNameMapping
            => (string)this["custIdJsonParamNameMapping"];

        public string[] CustIdJsonParamNames
        {
            get
            {
                custIdJsonParamNames = custIdJsonParamNames ??
                    CustIdJsonParamNameMapping.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return custIdJsonParamNames;
            }
        }

        [ConfigurationProperty("cachedExpireTime", IsRequired = true, DefaultValue = DefaultCachedExpireTime)]
        public int CachedExpireTime
            => (int)this["cachedExpireTime"];

        [ConfigurationProperty("kickOutUrl", DefaultValue = "~/auth/kickout")]
        public string KickoutUrl
            => (string)this["kickOutUrl"];

        [ConfigurationProperty("enableCheckBelonging", DefaultValue = true)]
        public bool EnableCheckBelonging
            => (bool)this["enableCheckBelonging"];
    }
}
