namespace CN88.Auth.Params.SecurityCode
{
    public class ChangeSecurityCodeParam : ResetSecurityCodeParam
    {
        public ChangeSecurityCodeParam(
               int custId,
               int subAccId,
               string newSecurityCode,
               int userId,
               string ip,
               string languageKey) : this(custId, subAccId, null, newSecurityCode, null, userId, ip, languageKey)
        {
            MustVerifyCurrentSecurityCode = false;
        }

        public ChangeSecurityCodeParam(
            int custId,
            int subAccId,
            string currentSecurityCode,
            string newSecurityCode,
            string salt,
            int userId,
            string ip,
            string languageKey) : base(custId, subAccId, newSecurityCode, userId, ip, languageKey)
        {
            CurrentSecurityCode = currentSecurityCode;
            Salt = salt;
            MustVerifyCurrentSecurityCode = true;
        }

        public bool MustVerifyCurrentSecurityCode { get; }

        public string CurrentSecurityCode { get; }

        public string Salt { get; }
    }
}
