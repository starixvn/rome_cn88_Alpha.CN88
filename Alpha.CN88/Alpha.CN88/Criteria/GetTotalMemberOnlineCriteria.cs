namespace Alpha.CN88.Criteria
{
    using Fanex.Data.Repository;

    public class GetTotalMemberOnlineCriteria : CriteriaBase
    {
        public GetTotalMemberOnlineCriteria(
            int custId,
            int subAccId,
            bool isAllDownline)
        {
            CustId = custId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public override string GetSettingKey()
            => "CS_Customer_TotalOnlineUser_Sel";

        public override bool IsValid() => CustId > 0;
    }
}