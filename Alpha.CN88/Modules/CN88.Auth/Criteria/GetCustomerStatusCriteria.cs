namespace CN88.Auth.Criteria
{
    using Fanex.Data.Repository;

    public class GetCustomerStatusCriteria : CriteriaBase
    {
        public GetCustomerStatusCriteria(int custId, int adminId, string system)
        {
            CustId = custId;
            AdminId = adminId;
            System = system;
        }

        public int CustId { get; }

        public int AdminId { get; }

        public string System { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelStatus";

        public override bool IsValid()
            => CustId > 0
            && AdminId >= 0;
    }
}
