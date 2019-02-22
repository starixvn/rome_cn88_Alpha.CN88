namespace CN88.Auth.Params.SecurityCode
{
    public class ResetSecurityCodeParam
    {
        public ResetSecurityCodeParam(
            int custId,
            int subAccId,
            string newSecurityCode,
            bool isResetMember,
            int userId,
            string ip,
            string languageKey)
        {
            CustId = custId;
            SubAccId = subAccId;
            NewSecurityCode = newSecurityCode;
            IsResetMember = isResetMember;
            UserId = userId;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string NewSecurityCode { get; }

        public bool IsResetMember { get; }

        public int UserId { get; }

        public string IP { get; }

        public string LanguageKey { get; }
    }
}
