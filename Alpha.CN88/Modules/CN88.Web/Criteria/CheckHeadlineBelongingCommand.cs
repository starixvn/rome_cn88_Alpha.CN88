namespace CN88.Web.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckHeadlineBelongingCommand : NonQueryCommand
    {
        internal CheckHeadlineBelongingCommand(
            int sessionCustId,
            int custId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
        }

        public int SessionCustId { get; }

        public int CustId { get; }

        public byte RoleId { get; set; }

        public override string GetSettingKey()
            => "CS_HeadlineSelCheckBelonging";

        public override bool IsValid()
            => SessionCustId > 0 && CustId > 0;
    }
}
