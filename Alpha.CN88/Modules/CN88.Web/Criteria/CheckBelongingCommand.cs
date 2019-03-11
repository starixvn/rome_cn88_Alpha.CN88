namespace CN88.Web.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckBelongingCommand : NonQueryCommand
    {
        internal CheckBelongingCommand(
            int sessionCustId,
            int subAccountId,
            long transId) : this(sessionCustId, null, subAccountId, transId)
        {
        }

        internal CheckBelongingCommand(
            int sessionCustId,
            int? custId,
            int subAccountId,
            long transId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
            SubAccountId = subAccountId;
            TransId = transId;
        }

        public int SessionCustId { get; }

        public int? CustId { get; }

        public int SubAccountId { get; }

        public long TransId { get; }

        public bool IsActive { get; set; }

        public string UserName { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerSelCheckBelonging";

        public override bool IsValid()
            => SessionCustId > 0;
    }
}
