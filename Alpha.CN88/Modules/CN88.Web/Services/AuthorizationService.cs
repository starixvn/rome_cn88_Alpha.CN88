namespace CN88.Web.Services
{
    using System;
    using System.Linq;
    using Configurations;
    using Models;

    public interface AuthorizationService
    {
        bool IsBelonging(BelongingContext belongingContext);

        bool IsAuthorized(string url);

        bool IsAuthorized(BelongingContext belongingContext);

        bool IsCustomerActive(BelongingContext belongingContext);

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

        public bool IsBelonging(BelongingContext belongingContext)
        {
            foreach (var groupPageAccess in belongingContext.GroupPageAccessList)
            {
                if (groupPageAccess.RoleId <= belongingContext.ActorRoleId)
                {
                    var pageAccess = groupPageAccess.PageAccesses
                        .Cast<PageAccessElement>()
                        .FirstOrDefault(item => item.IsCheckBelonging && belongingContext.IsCurrentUrlContains(item.Url));

                    if (pageAccess != null)
                    {
                        var custId = requestContext.GetCustId(
                            belongingContext.HttpRequest,
                            belongingContext.CustIdQueryStringParamNames,
                            belongingContext.CustIdJsonParamNames);
                        var transId = requestContext.GetTransId(belongingContext.HttpRequest);

                        return IsBelongingOnPage(custId, transId, pageAccess, belongingContext);
                    }
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

        public bool IsAuthorized(string url = null)
            => IsAuthorized(new BelongingContext());

        public bool IsAuthorized(BelongingContext belongingContext)
        {
            var subAccHasPermissionToViewUrl = belongingContext.SubAccountId == 0
                ? true
                : CheckSubAccHasPermissionToViewUrl(belongingContext);

            return HasPermissionToViewUrl(belongingContext) && subAccHasPermissionToViewUrl;
        }

        private bool CheckSubAccHasPermissionToViewUrl(BelongingContext belongingContext)
        {
            foreach (var groupPageAccess in belongingContext.GroupPageAccessList)
            {
                if (groupPageAccess.RoleId <= belongingContext.ActorRoleId)
                {
                    var pageAccess = groupPageAccess.PageAccesses
                        .Cast<PageAccessElement>()
                        .FirstOrDefault(item => belongingContext.IsCurrentUrlContains(item.Url));

                    if (pageAccess != null)
                    {
                        return IsAuthorizedOnPage(belongingContext.SubPageAccess, pageAccess);
                    }
                }
            }

            return true;
        }

        private static bool IsAuthorizedOnPage(string permissions, PageAccessElement pageElement)
            => pageElement.Permission
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Any(item => permissions.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

        private bool HasPermissionToViewUrl(BelongingContext belongingContext)
        {
            var deniedpages = DeniedPageHelper.GetDeniedPages(belongingContext.ActorRoleId);

            return !deniedpages.Any(i => belongingContext.IsCurrentUrlContains(i.Url));
        }

        public bool IsCustomerActive(BelongingContext belongingContext)
            => customerService.IsCustomerActive(
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
