namespace CN88.Auth.Criteria.SecurityCode
{
    public class UpdateSubAccountSecurityCodeCounterCriteria : UpdateMainAccountSecurityCodeCounterCriteria
    {
        public UpdateSubAccountSecurityCodeCounterCriteria(
            int custId,
            int subAccId,
            bool checkedSecurityCode,
            string ip,
            string language) : base(custId, checkedSecurityCode, ip, language)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_UpdateSubAccountSecurityCodeCounter";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
