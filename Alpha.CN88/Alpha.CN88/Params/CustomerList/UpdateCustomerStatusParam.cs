namespace Alpha.CN88.Params.CustomerList
{
    public class UpdateClosedParam : UpdateCustomerStatusParam
    {
        public UpdateClosedParam(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string actorName,
            bool closed) : base(searchAccountId, actorId, subAccountId, language, ip, actorName)
        {
            Closed = closed;
        }

        public bool Closed { get; }
    }

    public class UpdateSuspendedParam : UpdateCustomerStatusParam
    {
        public UpdateSuspendedParam(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string actorName,
            bool suspended) : base(searchAccountId, actorId, subAccountId, language, ip, actorName)
        {
            Suspended = suspended;
        }

        public bool Suspended { get; }

       
    }

    public class UpdateCustomerStatusParam
    {
        public UpdateCustomerStatusParam(
            int searchAccountId,
            int actorId,
            int subAccountId,
            string language,
            string ip,
            string actorName)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            ActorName = actorName;
        }

        public int SearchAccountId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public string ActorName { get; }
    }
}
