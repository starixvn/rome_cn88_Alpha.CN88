namespace Alpha.CN88.Criteria.CustomerList
{
    using Fanex.Data;

    public class UpdateCreditCriteria : UpdateCustomerStatusCriteria
    {
        public UpdateCreditCriteria(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip) : base(searchAccountId, actorId, subAccountId, language, ip)
        {
            CreditAmount = creditAmount;
        }


        [SpParam(SqlParamName = "UpdCredit")]
        public decimal CreditAmount { get; }

        public override string GetSettingKey() => "Age_CS_Customer_Credit_Upd";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0;
    }
}
