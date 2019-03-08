namespace CN88.Web.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    internal static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed")]
        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string MainAppPath => GetConfigValue("MainAppPath");

        public static bool DisabledCheckDuplicationLogin
            => GetBoolConfigValue("DisabledCheckDuplicationLogin");

        private static bool GetBoolConfigValue(string configName)
            => GetConfigValue(configName) == "1";

        private static string GetConfigValue(string configName)
            => AppSettingValues[configName];
    }
}
