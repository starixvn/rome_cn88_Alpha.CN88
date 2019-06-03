namespace CN88.Web.Services
{
    using System.Linq;
    using Configurations;
    using Models;

    public interface AuthorizationService
    {
        bool IsSubAccBelonging(BelongingContext belongingContext);

        bool IsBelonging(BelongingContext belongingContext);

        bool IsAuthorized(string url);

        bool IsAuthorized(BelongingContext belongingContext);

        CustomerStatus CheckCustomerStatus(BelongingContext belongingContext);

        bool IsKickedOut(BelongingContext belongingContext);
    }

    public class AuthorizationServiceImpl : AuthorizationService
    {
        private readonly RequestContext requestContext;
        private readonly CustomerService customerService;

        public AuthorizationServiceImpl(
            RequestContext requestContext = null,
            CustomerService customerService = null)
        {
            this.requestContext = requestContext ?? new RequestContextImpl();
            this.customerService = customerService ?? new CustomerServiceImpl();
        }

        public bool IsSubAccBelonging(BelongingContext belongingContext)
        {
            var subAccId = requestContext.GetCustId(
                belongingContext.HttpRequest,
                belongingContext.CustIdQueryStringParamNames,
                belongingContext.CustIdJsonParamNames);

            return customerService.IsSubAccBelonging(belongingContext.ActorId, subAccId);
        }


        public bool IsBelonging(BelongingContext belongingContext)
        {
            var pageAccess = GetMatchCurrentUrl(belongingContext);

            if (pageAccess != null && pageAccess.IsCheckBelonging)
            {
                var custId = requestContext.GetCustId(
                    belongingContext.HttpRequest,
                    belongingContext.CustIdQueryStringParamNames,
                    belongingContext.CustIdJsonParamNames);
                var transId = requestContext.GetTransId(belongingContext.HttpRequest);

                if (custId > 0 || transId > 0)
                {
                    return IsBelongingOnPage(custId, transId, pageAccess, belongingContext);
                }
            }

            return true;
        }

        private bool IsBelongingOnPage(int custId, long transId, PageAccessElement pageAccess, BelongingContext belongingContext)
        {
            // Allow customers to update by themselves.
            if (belongingContext.ActorId == custId)
            {
                return pageAccess.IsAllowToDoByThemselves;
            }

            BelongingInfo belongingInfo;

            if (belongingContext.ActorRoleId == 0)
            {
                belongingInfo = customerService.GetHeadlineBelongingInfo(belongingContext.ActorId, custId);

                return belongingInfo.IsValid();
            }

            belongingInfo = customerService.GetBelongingInfo(belongingContext.ActorId, custId, belongingContext.SubAccountId, transId);

            return belongingInfo.IsValid() && belongingInfo.RoleId < belongingContext.ActorRoleId;
        }

        public bool IsAuthorized(string url)
            => IsAuthorized(new BelongingContext());

        public bool IsAuthorized(BelongingContext belongingContext)
        {
            var accountHasPermissionToAccessUrl = GetAccountPermissionInstance(belongingContext.SubAccountId)
                .IsAuthorized(belongingContext, GetMatchCurrentUrl(belongingContext));

            if (accountHasPermissionToAccessUrl)
            {
                var deniedpages = DeniedPageHelper.GetDeniedPages(belongingContext.ActorRoleId);

                return !deniedpages.Any(item => belongingContext.IsCurrentUrlContains(item.Url));
            }

            return false;
        }

        private static PageAccessElement GetMatchCurrentUrl(BelongingContext belongingContext)
        {
            foreach (var groupPageAccess in belongingContext.GroupPageAccessList)
            {
                if (groupPageAccess.RoleId <= belongingContext.ActorRoleId)
                {
                    foreach (PageAccessElement pageAccess in groupPageAccess.PageAccesses)
                    {
                        if (belongingContext.IsCurrentUrlContains(pageAccess.Url))
                        {
                            return pageAccess;
                        }
                    }
                }
            }

            return null;
        }

        private static AccountPermission GetAccountPermissionInstance(int subAccountId)
        {
            if (subAccountId == 0)
            {
                return new AccountStatusPermission();
            }

            return new SubAccountPermission();
        }

        public CustomerStatus CheckCustomerStatus(BelongingContext belongingContext)
            => customerService.CheckCustomerStatus(
                belongingContext.ActorId,
                belongingContext.SubAccountId,
                belongingContext.CachedExpireTime);

        public bool IsKickedOut(BelongingContext belongingContext)
            => customerService.IsKickedOut(
                belongingContext.ActorId,
                belongingContext.SubAccountId,
                belongingContext.OnlineUserId,
                belongingContext.CachedExpireTime);
    }
}
