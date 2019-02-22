namespace CN88.Auth.Params.Authentication
{
    using System;

    public class AdminLoginParam
    {
        public AdminLoginParam()
        {
        }

        public AdminLoginParam(
            int adminId,
            string systemLogin,
            string custName,
            long timeTicks,
            string language,
            string languageKey,
            RequestInfo requestInfo)
        {
            AdminId = adminId;
            SystemLogin = systemLogin;
            CustName = custName;
            TimeTicks = timeTicks;
            Language = language;
            LanguageKey = languageKey;
            RequestInfo = requestInfo;
        }

        public int AdminId { get; }

        public string SystemLogin { get; }

        public string CustName { get; }

        public long TimeTicks { get; }

        public DateTime LoginTime => new DateTime(TimeTicks);

        public string Language { get; }

        public string LanguageKey { get; }

        public RequestInfo RequestInfo { get; }
    }
}
