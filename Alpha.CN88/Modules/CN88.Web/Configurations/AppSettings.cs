namespace CN88.Web.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    internal static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string SessionVerificationTokenCookie
            => GetConfigValue("SessionVerificationTokenCookie") ?? "__SessionVerificationToken_";

        public static bool EnableSessionValidation
            => GetConfigValue("EnableSessionValidation") != null && bool.Parse(GetConfigValue("EnableSessionValidation"));

        public static bool EnableKickOutWhenSessionInvalid
            => GetConfigValue("EnableKickOutWhenSessionInvalid") != null && bool.Parse(GetConfigValue("EnableKickOutWhenSessionInvalid"));

        public static bool EnableAntiXSRF
            => GetConfigValue("EnableAntiXSRF") != null && bool.Parse(GetConfigValue("EnableAntiXSRF"));

        public static string MainAppPath => GetConfigValue("MainAppPath");

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        public static string MessageAppPath => GetConfigValue("MessageAppPath");

        public static string FeaturesAppPath => GetConfigValue("FeaturesAppPath");

        public static string StaticResourcesVersion => GetConfigValue("AssetsApp:StaticResourcesVersion");

        internal static bool DisabledCheckDuplicationLogin => GetBoolConfigValue("DisabledCheckDuplicationLogin");

        private static bool GetBoolConfigValue(string configName)
            => GetConfigValue(configName) == "1";

        private static string GetConfigValue(string configName)
            => AppSettingValues[configName];
    }
}
