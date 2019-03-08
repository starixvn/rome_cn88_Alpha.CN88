namespace CN88.Web.Services
{
    using System;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface CustomerService
    {
        BelongingInfo GetBelongingInfo(int actorId, int custId, int subAccountId, long transId);

        BelongingInfo GetHeadlineBelongingInfo(int actorId, int custId);

        bool IsCustomerActive(int custId, int subAccId);

        bool IsCustomerActive(int custId, int subAccId, int cacheInSeconds);

        bool IsKickedOut(int onlineUserId);

        bool IsKickedOut(int custId, int subAccId, int onlineUserId, int cacheInSeconds);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private const string IsCustomerActiveCacheKey = "CN88.Web.Services.IsAccountActiveCacheKey_{0}_{1}";
        private const string RecheckinCacheKey = "CN88.Web.Services.RecheckinCacheKey_{0}_{1}_{2}";

        private static object isCustomerActiveSyncLock = new object();
        private static object recheckinSyncLock = new object();

        private readonly CacheService cacheService;
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(
            CacheService cacheService = null,
            IDynamicRepository dynamicRepository = null)
        {
            this.cacheService = cacheService ?? new CacheServiceImpl();
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public BelongingInfo GetBelongingInfo(int actorId, int custId, int subAccountId, long transId)
        {
            var command = custId == 0
                ? new CheckBelongingCommand(actorId, subAccountId, transId)
                : new CheckBelongingCommand(actorId, custId, subAccountId, transId);
            dynamicRepository.Execute(command);

            return new BelongingInfo(command.IsActive, command.RoleId);
        }

        public BelongingInfo GetHeadlineBelongingInfo(int actorId, int custId)
        {
            var command = new CheckHeadlineBelongingCommand(actorId, custId);
            dynamicRepository.Execute(command);

            return new BelongingInfo(command.IsActive, command.RoleId);
        }

        public bool IsCustomerActive(int custId, int subAccountId)
        {
            var command = new CheckCustomerActiveCommand(custId, subAccountId);
            dynamicRepository.Execute(command);

            return command.IsActive;
        }

        public bool IsCustomerActive(int custId, int subAccId, int cacheInSeconds)
        {
            bool isActive;
            var key = string.Format(IsCustomerActiveCacheKey, custId, subAccId);

            if (!cacheService.TryGet(key, out isActive))
            {
                lock (isCustomerActiveSyncLock)
                {
                    if (!cacheService.TryGet(key, out isActive))
                    {
                        isActive = IsCustomerActive(custId, subAccId);

                        SetAbsoluteExpiration(key, isActive, cacheInSeconds);
                    }
                }
            }

            return isActive;
        }

        public bool IsKickedOut(int onlineUserId)
        {
            if (onlineUserId <= 0)
            {
                return true;
            }

            var command = new ReCheckinCommand(onlineUserId);
            dynamicRepository.Execute(command);

            return command.IsKickedOut;
        }

        public bool IsKickedOut(int custId, int subAccId, int onlineUserId, int cacheInSeconds)
        {
            if (onlineUserId <= 0)
            {
                return true;
            }

            bool isKickedOut;
            string key = string.Format(RecheckinCacheKey, custId, subAccId, onlineUserId);

            if (!cacheService.TryGet(key, out isKickedOut))
            {
                lock (recheckinSyncLock)
                {
                    if (!cacheService.TryGet(key, out isKickedOut))
                    {
                        isKickedOut = IsKickedOut(onlineUserId);

                        SetAbsoluteExpiration(key, isKickedOut, cacheInSeconds);
                    }
                }
            }

            return isKickedOut;
        }

        private void SetAbsoluteExpiration<T>(string key, T value, int cacheInSeconds)
            => cacheService.Set(key, value, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheInSeconds)));
    }
}
