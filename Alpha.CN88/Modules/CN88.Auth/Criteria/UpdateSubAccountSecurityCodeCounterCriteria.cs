namespace CN88.Auth.Criteria
{
    public class UpdateSubAccountSecurityCodeCounterCriteria : SCNonQueryCommand
    {
        public int SubaccId { get; set; }

        public bool CheckedSecurityCode { get; set; }

        public override string GetSettingKey()
            => "Age_UpdateSubAccountSecurityCodeCounter";

        public override bool IsValid() => CustId > 0;
    }
}
