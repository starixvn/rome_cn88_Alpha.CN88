namespace Alpha.CN88.Services
{
    using System;
    using System.Collections.Generic;
    using Models.Home;

    public interface HomeService
    {
        Tuple<IEnumerable<AccountOverview>, IEnumerable<StatusSummary>> GetGeneralInformation(int custId, int subAccId, bool isAllDownline);

        IEnumerable<AccountingPeriod> GetAccountingPeriods();
    }
}