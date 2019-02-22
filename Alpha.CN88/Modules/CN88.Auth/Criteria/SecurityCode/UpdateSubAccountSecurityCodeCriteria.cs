namespace CN88.Auth.Criteria.SecurityCode
{
    using Fanex.Data;

    public class UpdateSubAccountSecurityCodeCriteria : SecurityCodeNonQueryCommand
    {
        public UpdateSubAccountSecurityCodeCriteria(
            int custId,
            int subAccId,
            string hashedSecurityCode,
            string ip,
            string language) : base(custId, ip, language)
        {
            SubAccId = subAccId;
            HashedSecurityCode = hashedSecurityCode;
        }

        public int SubAccId { get; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; }

        public override string GetSettingKey()
            => "Age_UpdateSubAccountSecurityCode";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(HashedSecurityCode);
    }
}
