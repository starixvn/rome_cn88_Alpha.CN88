namespace CN88.Web.Services
{
    using System;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface CustomerService
    {
        byte GetBelongingCustRoleId(int actorId, int actorSubAccountId, int custId);

        byte GetHeadlineBelongingCustRoleId(int actorId, int custId);

        CustomerStatus CheckCustomerStatus(int custId, int subAccId);

        CustomerStatus CheckCustomerStatus(int custId, int subAccId, int cacheInSeconds);

        bool IsKickedOut(int onlineUserId);

        bool IsKickedOut(int custId, int subAccId, int onlineUserId, int cacheInSeconds);

        bool IsSubAccBelonging(int custId, int subAccId);

        void Logout(LogoutContext logoutContext);
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

        public bool IsSubAccBelonging(int custId, int subAccId)
        {
            var subAccounts = dynamicRepository.Fetch<int>(new GetSubAccountListCriteria(custId));

            return subAccounts.Any(i => i == subAccId);
        }

        public byte GetBelongingCustRoleId(int actorId, int actorSubAccountId, int custId)
        {
            var command = new CheckBelongingCommand(actorId, actorSubAccountId, custId);
            dynamicRepository.Execute(command);

            return command.RoleId;
        }

        public byte GetHeadlineBelongingCustRoleId(int actorId, int custId)
        {
            var command = new CheckHeadlineBelongingCommand(actorId, custId);
            dynamicRepository.Execute(command);

            return command.RoleId;
        }

        public CustomerStatus CheckCustomerStatus(int custId, int subAccId)
        {
            var customerStatus = dynamicRepository.Get<CustomerStatus>(new CheckCustomerStatusCriteria(custId, subAccId));
            customerStatus.IsSubAccount = subAccId > 0;

            return customerStatus;
        }

        public CustomerStatus CheckCustomerStatus(int custId, int subAccId, int cacheInSeconds)
        {
            CustomerStatus customerStatus;
            var key = string.Format(IsCustomerActiveCacheKey, custId, subAccId);

            if (!cacheService.TryGet(key, out customerStatus))
            {
                lock (isCustomerActiveSyncLock)
                {
                    if (!cacheService.TryGet(key, out customerStatus))
                    {
                        customerStatus = CheckCustomerStatus(custId, subAccId);

                        SetAbsoluteExpiration(key, customerStatus, cacheInSeconds);
                    }
                }
            }

            return customerStatus;
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

        public void Logout(LogoutContext logoutContext)
            => dynamicRepository.Execute(new LogoutCommand(logoutContext));
    }
}
