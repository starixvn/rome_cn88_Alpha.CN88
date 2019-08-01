namespace Alpha.CN88.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Home;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Home;

    public class HomeServiceImpl : HomeService
    {
        private const string AccountingPeriodsCacheKey = "CN88.Features.Reports.Services.AccountingPeriodServiceImpl.AccountingPeriods";

        private readonly IDynamicRepository dynamicRepository;

        public HomeServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Tuple<IEnumerable<AccountOverview>, IEnumerable<StatusSummary>> GetGeneralInformation(int custId, int subAccId, bool isAllDownline)
        {
            var currentPeriod = GetCurrentPeriod();
            var criteria = new GetHomeInformationCriteria(
                custId,
                subAccId,
                isAllDownline,
                currentPeriod.FromDate,
                currentPeriod.ToDate);

            return dynamicRepository.FetchMultiple<AccountOverview, StatusSummary>(criteria);
        }

        public AccountingPeriod GetCurrentPeriod()
            => GetAccountingPeriods().FirstOrDefault(p => p.IsCurrentPeriod);

        public IEnumerable<AccountingPeriod> GetAccountingPeriods()
            => ConfiguredCacheService.GetOrSet(
                AccountingPeriodsCacheKey,
                () => dynamicRepository.Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}