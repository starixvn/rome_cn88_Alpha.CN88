namespace CN88.Auth.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    internal static class AuthAppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed")]
        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static bool DisabledCheckDuplicationLogin
            => GetBoolConfigValue("DisabledCheckDuplicationLogin");

        public static int CustomerStatusExpireTime
            => GetIntConfigValue("CustomerStatusExpireTime");

        private static bool GetBoolConfigValue(string configName)
            => GetConfigValue(configName) == "1";

        private static int GetIntConfigValue(string configName)
            => Convert.ToInt32(GetConfigValue(configName));

        private static string GetConfigValue(string configName)
            => AppSettingValues[configName];
    }
}
