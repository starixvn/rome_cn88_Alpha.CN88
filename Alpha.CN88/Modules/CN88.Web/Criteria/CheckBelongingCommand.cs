namespace CN88.Web.Criteria
{
    internal class CheckBelongingCommand : CheckHeadlineBelongingCommand
    {
        internal CheckBelongingCommand(
            int sessionCustId,
            int custId,
            int subAccountId) : base(sessionCustId, custId)
        {
            SubAccountId = subAccountId;
        }

        public int SubAccountId { get; }

        public override string GetSettingKey()
            => "CS_CustomerSelCheckBelonging";
    }
}
