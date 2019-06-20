namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using Models;
    using Services;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class CheckBelongingFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserSession.Current?.CustId == 0 ||
                filterContext.Result is HttpUnauthorizedResult)
            {
                UnAuthorizationHelper.UnauthorizeOrLogin(filterContext);
                return;
            }

            var belongingContext = new BelongingContext(
                httpRequest: filterContext.HttpContext.Request,
                currentUrl: GetCurrentUrl(filterContext));

            if (belongingContext.IsValid)
            {
                AuthorizationService authorizationService = new AuthorizationServiceImpl();

                if (belongingContext.EnableCheckBelonging)
                {
                    if (!authorizationService.IsBelonging(belongingContext))
                    {
                        UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                        return;
                    }

                    if (!authorizationService.IsAuthorized(belongingContext))
                    {
                        UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                        return;
                    }
                }

                if (belongingContext.EnableKickout)
                {
                    var customerStatus = authorizationService.CheckCustomerStatus(belongingContext);

                    if (customerStatus.IsKickedOut(belongingContext.CustomerStatus))
                    {
                        authorizationService.Logout(belongingContext);
                        UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, customerStatus.GetErrorResourceKey());
                        return;
                    }

                    if (!AppSettings.DisabledCheckDuplicationLogin
                        && authorizationService.IsKickedOut(belongingContext))
                    {
                        authorizationService.Logout(belongingContext);
                        UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "SessionKickedOut");
                    }
                }
            }
        }

        private static string GetCurrentUrl(AuthorizationContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;

            return $"{actionDescriptor.ControllerDescriptor.ControllerName}/{actionDescriptor.ActionName}";
        }
    }
}
