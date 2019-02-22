namespace CN88.Auth.Criteria
{
    public class UpdateMainAccountSecurityCodeCounterCriteria : SCNonQueryCommand
    {
        public bool CheckedSecurityCode { get; set; }

        public override string GetSettingKey()
            => "Age_UpdateMainAccountSecurityCodeCounter";

        public override bool IsValid() => CustId > 0;
    }
}
