namespace Alpha.CN88.Criteria
{
    using Fanex.Data.Repository;

    public class GetTotalMemberOnlineCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_TotalOnlineUser_Sel";

        public override bool IsValid() => CustId > 0;
    }
}