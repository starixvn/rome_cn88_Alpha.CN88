namespace Alpha.CN88.Criteria.CustomerList
{
    public class UpdateSuspendedCriteria : UpdateCustomerStatusCriteria
    {
        public UpdateSuspendedCriteria(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            bool suspended) : base(searchAccountId, actorId, subAccountId, language, ip)
        {
            Suspended = suspended;
        }

        public bool Suspended { get; }

        public override string GetSettingKey() => "Acc_CustomerUpdSuspended";
    }
}
