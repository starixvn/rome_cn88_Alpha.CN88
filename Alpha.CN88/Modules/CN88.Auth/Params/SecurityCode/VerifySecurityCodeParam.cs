namespace CN88.Auth.Params.SecurityCode
{
    public class VerifySecurityCodeParam
    {
        public VerifySecurityCodeParam(
            int custId,
            int subAccId,
            string securityCode,
            string salt,
            string ip,
            string languageKey)
        {
            CustId = custId;
            SubAccId = subAccId;
            SecurityCode = securityCode;
            Salt = salt;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string SecurityCode { get; }

        public string Salt { get; }

        public string IP { get; }

        public string LanguageKey { get; }
    }
}
