namespace Alpha.CN88.Criteria.Home
{
    using Fanex.Data.Repository;

    public class GetGetAccountOverviewCriteria : CriteriaBase
    {
        public GetGetAccountOverviewCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "CS_AccountingPeriod_Sel";

        public override bool IsValid() => true;
    }
}
