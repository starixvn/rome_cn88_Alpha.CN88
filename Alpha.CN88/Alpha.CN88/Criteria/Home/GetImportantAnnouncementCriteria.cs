namespace Alpha.CN88.Criteria.Home
{
    using Fanex.Data.Repository;

    public class GetImportantAnnouncementCriteria : CriteriaBase
    {
        public GetImportantAnnouncementCriteria(int custId, string language)
        {
            CustId = custId;
            Language = language;
        }

        public int CustId { get; }

        public string Language { get; }

        public override string GetSettingKey() => "CS_AccountingPeriod_Sel";

        public override bool IsValid() => true;
    }
}
