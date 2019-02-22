namespace CN88.Auth.Criteria.Authentication
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class LoginCommand : NonQueryCommand
    {
        public LoginCommand(
            int custId,
            string password,
            int subAccId,
            DateTime loginTime,
            string nickName,
            bool isNickName,
            string languageKey,
            string loginIP,
            string loginDomain,
            string browserName,
            string platform,
            string countryCode)
        {
            CustId = custId;
            Password = password;
            SubAccId = subAccId;
            LoginTime = loginTime;
            NickName = nickName;
            IsNickName = IsNickName;
            LanguageKey = languageKey;
            LoginIP = loginIP;
            LoginDomain = loginDomain;
            BrowserName = browserName;
            Platform = platform;
            CountryCode = countryCode;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; }

        public int SubAccId { get; }

        public DateTime LoginTime { get; }

        public string NickName { get; }

        public bool IsNickName { get; }

        [SpParam(SqlParamName = "Language")]
        public string LanguageKey { get; }

        public string LoginIP { get; }

        public string LoginDomain { get; }

        public string BrowserName { get; }

        public string Platform { get; }

        public string CountryCode { get; }

        public override string GetSettingKey()
            => "Age_LoginClaimDownline";

        public override bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(Password)
            && SubAccId >= 0;
    }
}
