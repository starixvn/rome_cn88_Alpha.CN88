namespace Alpha.CN88.Params.CustomerList
{
    public class UpdateCreditParam
    {
        public UpdateCreditParam(
           int searchAccountId,
           byte roleId,
           decimal creditAmount,
           int actorId,
           int subAccountId,
           string language,
           string ip)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            RoleId = roleId;
            CreditAmount = creditAmount;
        }

        public int SearchAccountId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public byte RoleId { get; }

        public decimal CreditAmount { get; }
    }
}
