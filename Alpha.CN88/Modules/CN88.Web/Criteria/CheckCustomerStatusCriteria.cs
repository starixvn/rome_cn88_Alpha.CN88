namespace CN88.Web.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckCustomerStatusCriteria : CriteriaBase
    {
        internal CheckCustomerStatusCriteria(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "CS_CustomerStatusSel";

        public override bool IsValid()
            => CustId > 0
            && SubAccId >= 0;
    }
}
