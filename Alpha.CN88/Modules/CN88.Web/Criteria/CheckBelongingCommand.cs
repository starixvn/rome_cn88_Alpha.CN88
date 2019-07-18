namespace CN88.Web.Criteria
{
    internal class CheckBelongingCommand : CheckHeadlineBelongingCommand
    {
        internal CheckBelongingCommand(int actorId, int actorSubAccountId, int custId)
            : base(actorId, custId)
        {
            SubAccountId = actorSubAccountId;
        }

        public int SubAccountId { get; }

        public override string GetSettingKey()
            => "CS_CustomerSelCheckBelonging";
    }
}
