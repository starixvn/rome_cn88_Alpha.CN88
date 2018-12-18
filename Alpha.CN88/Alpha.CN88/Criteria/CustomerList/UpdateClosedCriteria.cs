namespace Alpha.CN88.Criteria.CustomerList
{
    public class UpdateClosedCriteria : UpdateCustomerStatusCmd
    {
        public UpdateClosedCriteria(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            bool closed) : base(searchAccountId, actorId, subAccountId, language, ip)
        {
            Closed = closed;
        }

        public bool Closed { get; }

        public override string GetSettingKey() => "Acc_CustomerUpdClosed";
    }
}
