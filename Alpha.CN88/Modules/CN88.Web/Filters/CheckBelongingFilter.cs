namespace CN88.Web.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using Models;
    using Services;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class CheckBelongingFilter : AuthorizeAttribute
    {
        private static readonly AuthorizationSection AuthorizationSectionInstance = AuthorizationSection.Instance;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IUserSession userSession = new UserSessionExt();

            if (userSession?.CustId == 0 || filterContext.Result is HttpUnauthorizedResult)
            {
                UnAuthorizationHelper.UnauthorizeOrLogin(filterContext);
                return;
            }

            if (userSession.IsInternal)
            {
                return;
            }

            var currentURL = GetCurrentUrl(filterContext);
            var request = filterContext.HttpContext.Request;
            var authorizationService = GetAuthorizationService();

            // 1. Check belonging.
            if (AuthorizationSectionInstance.EnableCheckBelonging)
            {
                var belongingParam = new BelongingParam(currentURL, userSession);

                if (!authorizationService.IsBelonging(request, belongingParam))
                {
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                    return;
                }
            }

            if (EnableKickout)
            {
                // 2.1. Check account status is Active can do Update action.
                // 2.2. Check sub account permission.
                var accountPermissionParam = new AccountPermissionParam(currentURL, IsHttpPost(request), userSession);

                if (!authorizationService.IsAccountHasPermissionOnURL(accountPermissionParam))
                {
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                    return;
                }

                // 3. Check account status is changed by Admin or Upline.
                var customerStatus = authorizationService.CheckCustomerStatus(
                    accountPermissionParam.ActorId,
                    accountPermissionParam.ActorSubAccountId);

                if (customerStatus.IsKickedOut(accountPermissionParam.ActorStatus))
                {
                    authorizationService.Logout(new LogoutContext(userSession));
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, customerStatus.GetErrorResourceKey());
                    return;
                }

                // 4. Check duplication login.
                if (!AppSettings.DisabledCheckDuplicationLogin
                    && authorizationService.IsKickedOut(accountPermissionParam.ActorId, accountPermissionParam.ActorSubAccountId, userSession.OnUserId))
                {
                    authorizationService.Logout(new LogoutContext(userSession));
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "SessionKickedOut");
                }
            }
        }

        private static AuthorizationService GetAuthorizationService()
            => new AuthorizationServiceImpl(authorizationSection: AuthorizationSectionInstance);

        private static string GetCurrentUrl(AuthorizationContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;

            return $"{actionDescriptor.ControllerDescriptor.ControllerName}/{actionDescriptor.ActionName}";
        }

        private static bool IsHttpPost(HttpRequestBase request)
            => request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);

        private static bool EnableKickout
            => !string.IsNullOrWhiteSpace(AuthorizationSectionInstance.KickoutUrl);
    }
}
