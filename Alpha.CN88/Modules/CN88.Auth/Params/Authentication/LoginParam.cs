namespace CN88.Auth.Params.Authentication
{
    using System;

    public class LoginParam
    {
        public LoginParam()
        {
        }

        public LoginParam(
            int custId,
            string password,
            int subAccId,
            long timeTicks,
            string nickName,
            bool isNickName,
            string language,
            string languageKey,
            RequestInfo requestInfo,
            string deviceId)
        {
            CustId = custId;
            Password = password;
            SubAccId = subAccId;
            TimeTicks = timeTicks;
            NickName = nickName;
            IsNickName = isNickName;
            Language = language;
            LanguageKey = languageKey;
            RequestInfo = requestInfo;
            DeviceId = deviceId;
        }

        public int CustId { get; }

        public string Password { get; }

        public int SubAccId { get; }

        public long TimeTicks { get; }

        public DateTime LoginTime => new DateTime(TimeTicks);

        public string NickName { get; }

        public bool IsNickName { get; }

        public string Language { get; }

        public string LanguageKey { get; }

        public RequestInfo RequestInfo { get; }

        public string DeviceId { get; }
    }
}
