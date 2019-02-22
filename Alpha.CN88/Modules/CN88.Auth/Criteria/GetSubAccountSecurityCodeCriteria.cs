namespace CN88.Auth.Criteria
{
    using Fanex.Data.Repository;

    public class GetSubAccountSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubaccId { get; set; }

        public override string GetSettingKey()
            => "Age_GetSubAccountSecurityCode";

        public override bool IsValid() => CustId > 0;
    }
}
