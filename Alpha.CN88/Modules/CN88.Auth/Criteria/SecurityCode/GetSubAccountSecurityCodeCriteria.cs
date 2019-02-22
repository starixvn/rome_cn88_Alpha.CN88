namespace CN88.Auth.Criteria.SecurityCode
{
    public class GetSubAccountSecurityCodeCriteria : GetMainAccountSecurityCodeCriteria
    {
        public GetSubAccountSecurityCodeCriteria(
            int custId,
            int subAccId) : base(custId)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_GetSubAccountSecurityCode";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
