namespace CN88.Auth.Params.SecurityCode
{
    public class ChangeSecurityCodeParam : ResetSecurityCodeParam
    {
        public ChangeSecurityCodeParam(
            int custId,
            int subAccId,
            string currentSecurityCode,
            string newSecurityCode,
            string salt,
            bool isResetMember,
            bool mustVerifyCurrentSecurityCode,
            int userId,
            string ip,
            string languageKey) : base(custId, subAccId, newSecurityCode, isResetMember, userId, ip, languageKey)
        {
            CurrentSecurityCode = currentSecurityCode;
            Salt = salt;
            MustVerifyCurrentSecurityCode = mustVerifyCurrentSecurityCode;
        }

        public bool MustVerifyCurrentSecurityCode { get; }

        public string CurrentSecurityCode { get; }

        public string Salt { get; }
    }
}
