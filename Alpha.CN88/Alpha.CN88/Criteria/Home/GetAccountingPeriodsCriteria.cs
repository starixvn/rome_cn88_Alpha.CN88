namespace Alpha.CN88.Criteria.Home
{
    using System;
    using Fanex.Data.Repository;

    public class GetAccountingPeriodsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_AccountingPeriod_Sel";

        public override bool IsValid() => true;
    }
}
