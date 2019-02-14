namespace Alpha.CN88.Params.Authentication
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
            DateTime loginTime,
            string language,
            string langKey,
            string browserSize)
        {
            AdminId = adminId;
            SystemLogin = systemLogin;
            CustName = custName;
            TimeTicks = timeTicks;
            LoginTime = loginTime;
            Language = language;
            LangKey = langKey;
            BrowserSize = browserSize;
        }

        public int AdminId { get; }

        public string SystemLogin { get; }

        public string CustName { get; }

        public long TimeTicks { get; }

        public DateTime LoginTime { get; }

        public string Language { get; }

        public string LangKey { get; }

        public string BrowserSize { get; }
    }
}
