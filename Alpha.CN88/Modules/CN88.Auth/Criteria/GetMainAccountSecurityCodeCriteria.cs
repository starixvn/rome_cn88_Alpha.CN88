namespace CN88.Auth.Criteria
{
    using Fanex.Data.Repository;

    public class GetMainAccountSecurityCodeCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "Age_GetMainAccountSecurityCode";

        public override bool IsValid() => CustId > 0;
    }
}
