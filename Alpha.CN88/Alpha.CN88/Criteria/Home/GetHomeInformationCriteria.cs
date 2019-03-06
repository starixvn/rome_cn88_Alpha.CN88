namespace Alpha.CN88.Criteria.Home
{
    using System;
    using Fanex.Data.Repository;

    public class GetHomeInformationCriteria : CriteriaBase
    {
        public GetHomeInformationCriteria(int custId, DateTime fromDate, DateTime toDate)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override string GetSettingKey() => "CS_Customer_Homepage_Sel";

        public override bool IsValid()
            => CustId > 0 &&
            FromDate > DateTime.MinValue &&
            FromDate < ToDate &&
            ToDate < DateTime.MaxValue;
    }
}
