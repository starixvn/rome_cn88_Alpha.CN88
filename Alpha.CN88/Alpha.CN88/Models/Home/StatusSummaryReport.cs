namespace Alpha.CN88.Models.Home
{
    using System.Collections.Generic;

    public class StatusSummaryReport
    {
        public StatusSummaryReport(IList<StatusSummary> statusSummary)
        {
            StatusSummary = statusSummary;
            Total = GetTotal();
        }

        public IList<StatusSummary> StatusSummary { get; }

        public StatusSummary Total { get; }

        private StatusSummary GetTotal()
        {
            var result = new StatusSummary();
            foreach (var s in StatusSummary)
            {
                result.Add(s);
            }

            return result;
        }
    }
}
