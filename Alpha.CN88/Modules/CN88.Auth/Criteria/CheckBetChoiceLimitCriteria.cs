namespace CN88.Auth.Criteria
{
    using Fanex.Data.Repository;

    public class CheckBetChoiceLimitCriteria : CriteriaBase
    {
        public CheckBetChoiceLimitCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Age_CustSetting_BetChoiceLimitCheck";

        public override bool IsValid()
            => CustId > 0;
    }
}
