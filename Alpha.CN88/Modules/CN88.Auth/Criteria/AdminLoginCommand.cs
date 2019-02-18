namespace CN88.Auth.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AdminLoginCommand : NonQueryCommand
    {
        public AdminLoginCommand(
            string userName,
            int adminId,
            DateTime loginTime,
            string languageKey,
            string loginIP,
            string loginDomain,
            string browserName,
            string platform,
            string countryCode)
        {
            UserName = userName;
            AdminId = adminId;
            LoginTime = loginTime;
            LanguageKey = languageKey;
            LoginIP = loginIP;
            LoginDomain = loginDomain;
            BrowserName = browserName;
            Platform = platform;
            CountryCode = countryCode;
        }

        public string UserName { get; }

        [SpParam(SqlParamName = "AccUserID")]
        public int AdminId { get; }

        public DateTime LoginTime { get; }

        [SpParam(SqlParamName = "Language")]
        public string LanguageKey { get; }

        public string LoginIP { get; }

        public string LoginDomain { get; }

        public string BrowserName { get; }

        public string Platform { get; }

        public string CountryCode { get; }

        public override string GetSettingKey()
            => "Age_LoginDownline_Admin";

        public override bool IsValid()
            => string.IsNullOrWhiteSpace(UserName)
            && AdminId > 0;
    }
}
