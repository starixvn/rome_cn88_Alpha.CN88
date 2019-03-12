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

        internal static string MainAppPath => GetConfigValue("MainAppPath");

        internal static bool DisabledCheckDuplicationLogin
            => GetBoolConfigValue("DisabledCheckDuplicationLogin");

        internal static bool IsIntegrationTest
            => GetBoolConfigValue("IsIntegrationTest");

        private static bool GetBoolConfigValue(string configName)
            => GetConfigValue(configName) == "1";

        private static string GetConfigValue(string configName)
            => AppSettingValues[configName];
    }
}
