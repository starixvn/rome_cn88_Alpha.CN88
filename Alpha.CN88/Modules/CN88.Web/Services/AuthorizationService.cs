namespace CN88.Web.Services
{
    using System;
    using System.Linq;
    using System.Web;
    using Configurations;
    using Fanex.Logging.Extensions.Web;
    using Models;
    using Sunplus.Agent.Model.Entities;

    public interface AuthorizationService
    {
        bool IsSubAccBelonging(HttpRequestBase httpRequest, int actorId);

        bool IsBelonging(HttpRequestBase httpRequest, BelongingParam belongingParam);

        bool IsAccountHasPermissionOnURL(AccountPermissionParam accountPermissionParam);

        bool IsAuthorized(string currentURL);

        CustomerStatus CheckCustomerStatus(int custId, int subAccountId);

        bool IsKickedOut(int custId, int subAccountId, int onlineUserId);

        void Logout(LogoutContext logoutContext);
    }

    public class AuthorizationServiceImpl : AuthorizationService
    {
        private const byte MemberRoleId = 1;
        private const byte SuperRoleId = 4;

        private readonly RequestContext requestContext;
        private readonly CustomerService customerService;
        private readonly AuthorizationSection authorizationSection;

        public AuthorizationServiceImpl(
            RequestContext requestContext = null,
            CustomerService customerService = null,
            AuthorizationSection authorizationSection = null)
        {
            this.requestContext = requestContext ?? new RequestContextImpl();
            this.customerService = customerService ?? new CustomerServiceImpl();
            this.authorizationSection = authorizationSection ?? AuthorizationSection.Instance;
        }

        public bool IsSubAccBelonging(HttpRequestBase httpRequest, int actorId)
        {
            var subAccId = requestContext.GetCustIdFromHttpRequest(
                httpRequest,
                authorizationSection.CustIdQueryStringParamNames,
                authorizationSection.CustIdJsonParamNames);

            return customerService.IsSubAccBelonging(actorId, subAccId);
        }

        public bool IsBelonging(HttpRequestBase httpRequest, BelongingParam belongingParam)
        {
            var pageAccess = GetMatchPageAccessByURL(belongingParam.ActorRoleId, belongingParam.CurrentURL);

            if (pageAccess?.IsCheckBelonging == true)
            {
                var custId = requestContext.GetCustIdFromHttpRequest(
                    httpRequest,
                    authorizationSection.CustIdQueryStringParamNames,
                    authorizationSection.CustIdJsonParamNames);

                return IsBelonging(custId, belongingParam, pageAccess);
            }

            return true;
        }

        private bool IsBelonging(int custId, BelongingParam belongingParam, PageAccessElement pageAccess)
        {
            if (custId <= 0)
            {
                return true;
            }

            if (belongingParam.ActorId == custId)
            {
                return pageAccess.IsAllowToDoByThemselves;
            }

            byte custRoleId;

            if (belongingParam.IsHeadline)
            {
                custRoleId = customerService.GetHeadlineBelongingCustRoleId(belongingParam.ActorId, custId);

                return IsValidRoleId(custRoleId);
            }

            custRoleId = customerService.GetBelongingCustRoleId(
                belongingParam.ActorId,
                belongingParam.ActorSubAccountId,
                custId);

            return IsValidRoleId(custRoleId) && custRoleId < belongingParam.ActorRoleId;
        }

        private static bool IsValidRoleId(byte roleId)
            => MemberRoleId <= roleId && roleId <= SuperRoleId;

        public bool IsAccountHasPermissionOnURL(AccountPermissionParam accountPermissionParam)
        {
            var pageAccess = GetMatchPageAccessByURL(accountPermissionParam.ActorRoleId, accountPermissionParam.CurrentURL);

            if (pageAccess == null)
            {
                return true;
            }

            if (IsRestrictedURL(accountPermissionParam.CurrentURL, accountPermissionParam.ActorRoleId)
                || IsInvalidAccountStatus(accountPermissionParam, pageAccess))
            {
                return false;
            }

            return accountPermissionParam.ActorIsMainAccount
                || IsValidSubAccountPermission(accountPermissionParam.ActorSubPageAccess, pageAccess);
        }

        private static bool IsRestrictedURL(string currentURL, byte actorRoleId)
        {
            var deniedpages = DeniedPageHelper.GetDeniedPages(actorRoleId);

            return deniedpages.Any(item => currentURL.IndexOf(item.Url, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool IsInvalidAccountStatus(AccountPermissionParam accountPermissionParam, PageAccessElement pageAccess)
            => pageAccess.IsCheckAccountStatus && accountPermissionParam.IsHttpPost && accountPermissionParam.ActorStatusIsNotActive;

        private static bool IsValidSubAccountPermission(PageAccess actorSubPageAccess, PageAccessElement pageAccess)
            => pageAccess
                .Permission.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(item => actorSubPageAccess.GetPermission(item));

        public bool IsAuthorized(string currentURL)
            => string.IsNullOrWhiteSpace(currentURL)
            || IsAccountHasPermissionOnURL(new AccountPermissionParam(currentURL, false));

        private PageAccessElement GetMatchPageAccessByURL(byte actorRoleId, string currentURL)
        {
            var groupPageAccessList = authorizationSection.GroupPageAccessList;

            foreach (var groupPageAccess in groupPageAccessList)
            {
                if (groupPageAccess.RoleId <= actorRoleId)
                {
                    foreach (PageAccessElement pageAccess in groupPageAccess.PageAccesses)
                    {
                        if (currentURL.IndexOf(pageAccess.Url, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return pageAccess;
                        }
                    }
                }
            }

            return null;
        }

        public CustomerStatus CheckCustomerStatus(int custId, int subAccountId)
            => customerService.CheckCustomerStatus(custId, subAccountId, authorizationSection.CachedExpireTime);

        public bool IsKickedOut(int custId, int subAccountId, int onlineUserId)
            => customerService.IsKickedOut(custId, subAccountId, onlineUserId, authorizationSection.CachedExpireTime);

        public void Logout(LogoutContext logoutContext)
        {
            try
            {
                customerService.Logout(logoutContext);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
