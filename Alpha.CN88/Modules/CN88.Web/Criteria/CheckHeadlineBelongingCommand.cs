namespace CN88.Web.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckHeadlineBelongingCommand : NonQueryCommand
    {
        public CheckHeadlineBelongingCommand(
            int sessionCustId,
            int custId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
        }

        public int SessionCustId { get; }

        public int CustId { get; }

        public bool IsActive { get; set; }

        public string UserName { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_CheckBelonging_Sel";

        public override bool IsValid()
            => SessionCustId > 0 && CustId > 0;
    }
}
