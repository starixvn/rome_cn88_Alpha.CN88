namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using Models;
    using Services;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class CheckBelongingFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
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
                        UnAuthorizationHelper.ProcessUnAuthorization(filterContext, "AccessDeny");
                        return;
                    }

                    if (!authorizationService.IsAuthorized(belongingContext))
                    {
                        UnAuthorizationHelper.ProcessUnAuthorization(filterContext, "AccessDeny");
                        return;
                    }
                }

                if (belongingContext.EnableKickout)
                {
                    if (!authorizationService.IsCustomerActive(belongingContext))
                    {
                        UnAuthorizationHelper.ProcessUnAuthorization(filterContext, "CustomerClosed");
                        return;
                    }

                    if (!AppSettings.DisabledCheckDuplicationLogin
                        && authorizationService.IsKickedOut(belongingContext))
                    {
                        UnAuthorizationHelper.ProcessUnAuthorization(filterContext, "SessionKickedOut");
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
