namespace Alpha.CN88.Models.Home
{
    using System;
    using Alpha.CN88.Helpers;

    public class AccountingPeriod
    {
        public int PeriodId { get; }

        public int Year { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsCurrentPeriod => FromDate <= SystemTime.Today && SystemTime.Today <= ToDate;

        public AccountingPeriod()
        {
            FromDate = SystemTime.Today;
            ToDate = FromDate;
            Year = FromDate.Year;
        }
    }
}
