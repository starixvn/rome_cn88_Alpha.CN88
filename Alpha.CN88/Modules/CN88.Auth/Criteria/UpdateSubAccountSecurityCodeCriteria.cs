namespace CN88.Auth.Criteria
{
    using Fanex.Data;

    public class UpdateSubAccountSecurityCodeCriteria : SCNonQueryCommand
    {
        public int SubaccId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }

        public override string GetSettingKey()
            => "Age_UpdateSubAccountSecurityCode";

        public override bool IsValid() => CustId > 0;
    }
}
