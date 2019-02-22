namespace CN88.Auth.Criteria.SecurityCode
{
    public class UpdateMainAccountSecurityCodeCriteria : UpdateSubAccountSecurityCodeCriteria
    {
        public UpdateMainAccountSecurityCodeCriteria(
            int custId,
            int subAccId,
            string hashedSecurityCode,
            int userId,
            string ip,
            string language) : base(custId, subAccId, hashedSecurityCode, ip, language)
        {
            UserId = userId;
        }

        public int UserId { get; }

        public override string GetSettingKey()
            => "Age_UpdateMainAccountSecurityCode";

        public override bool IsValid()
            => base.IsValid() && UserId > 0;
    }
}
