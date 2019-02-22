namespace CN88.Auth.Criteria.SecurityCode
{
    public class ResetSubAccountSecurityCodeCriteria : UpdateMainAccountSecurityCodeCriteria
    {
        public ResetSubAccountSecurityCodeCriteria(
            int custId,
            int subAccId,
            string hashedSecurityCode,
            int userId,
            string ip,
            string language) : base(custId, subAccId, hashedSecurityCode, userId, ip, language)
        {
        }

        public override string GetSettingKey()
            => "Age_ResetSubAccountSecurityCode";
    }
}
