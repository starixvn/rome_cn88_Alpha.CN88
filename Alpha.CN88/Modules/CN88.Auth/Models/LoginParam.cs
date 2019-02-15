namespace CN88.Auth.Models
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
            string languageKey,
            RequestInfo requestInfo,
            string deviceId)
        {
            CustId = custId;
            Password = password;
            SubAccId = subAccId;
            TimeTicks = timeTicks;
            NickName = nickName;
            IsNickName = IsNickName;
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

        public string LanguageKey { get; }

        public RequestInfo RequestInfo { get; }

        public string DeviceId { get; }
    }
}
