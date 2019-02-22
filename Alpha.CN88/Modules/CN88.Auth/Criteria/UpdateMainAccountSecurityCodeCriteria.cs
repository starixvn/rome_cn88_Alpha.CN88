namespace CN88.Auth.Criteria
{
    using Fanex.Data;

    public class UpdateMainAccountSecurityCodeCriteria : SCNonQueryCommand
    {
        public int SubaccId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "securitycode")]
        public string HashedSecurityCode { get; set; }

        public override string GetSettingKey()
            => "Age_UpdateMainAccountSecurityCode";

        public override bool IsValid() => CustId > 0;
    }
}
