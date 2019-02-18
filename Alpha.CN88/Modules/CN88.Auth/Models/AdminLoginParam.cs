namespace CN88.Auth.Models
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
            string languageKey,
            RequestInfo requestInfo)
        {
            AdminId = adminId;
            SystemLogin = systemLogin;
            CustName = custName;
            TimeTicks = timeTicks;
            LanguageKey = languageKey;
            RequestInfo = requestInfo;
        }

        public int AdminId { get; }

        public string SystemLogin { get; }

        public string CustName { get; }

        public long TimeTicks { get; }

        public DateTime LoginTime => new DateTime(TimeTicks);

        public string LanguageKey { get; }

        public RequestInfo RequestInfo { get; }
    }
}
