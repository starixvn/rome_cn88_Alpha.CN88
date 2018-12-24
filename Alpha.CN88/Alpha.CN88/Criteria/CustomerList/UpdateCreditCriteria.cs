namespace Alpha.CN88.Criteria.CustomerList
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateCreditCriteria : NonQueryCommand
    {
        public UpdateCreditCriteria(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip)
        {
            CreditAmount = creditAmount;
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; }

        [SpParam(SqlParamName = "UpdCredit")]
        public decimal CreditAmount { get; }

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CS_Customer_Credit_Upd";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0;
    }
}
