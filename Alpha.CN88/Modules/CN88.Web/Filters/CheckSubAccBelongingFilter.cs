namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using Models;
    using Services;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CheckSubAccBelongingFilter : AuthorizeAttribute
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

                if (!authorizationService.IsSubAccBelonging(belongingContext))
                {
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                    return;
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
