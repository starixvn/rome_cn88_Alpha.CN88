namespace Alpha.CN88.Params.Authentication
{
    using System;

    public class LoginParam
    {
        public LoginParam()
        {
        }

        public LoginParam(
            int custId,
            int subAccId,
            long timeTicks,
            DateTime loginTime,
            string password,
            string nickName,
            bool isNickName,
            string language,
            string langKey,
            string deviceId,
            string browserSize)
        {
            CustId = custId;
            SubAccId = subAccId;
            TimeTicks = timeTicks;
            LoginTime = loginTime;
            Password = password;
            NickName = nickName;
            IsNickName = isNickName;
            Language = language;
            LangKey = langKey;
            DeviceId = deviceId;
            BrowserSize = browserSize;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public long TimeTicks { get; }

        public DateTime LoginTime { get; }

        public string Password { get; }

        public string NickName { get; }

        public bool IsNickName { get; }

        public string Language { get; }

        public string LangKey { get; }

        public string DeviceId { get; }

        public string BrowserSize { get; }
    }
}
