namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Helpers;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AfterLoginFlowFilter : ActionFilterAttribute
    {
        private readonly IUserSession userSession = new UserSessionExt();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (IsValidSession(filterContext))
            {
                var redirectUrl = GetAfterLoginFlowUrl(filterContext);

                if (redirectUrl != null)
                {
                    filterContext.Result = new RedirectResult(MicroSites.MainUrl(redirectUrl));
                }
            }
        }

        private bool IsValidSession(ActionExecutingContext filterContext)
            => filterContext.HttpContext?.Session != null
            && userSession.UserId > 0
            && !userSession.IsInternal;

        private string GetAfterLoginFlowUrl(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;

            if (!controllerName.Equals("auth", StringComparison.OrdinalIgnoreCase))
            {
                var currentUrl = $"{controllerName}/{actionDescriptor.ActionName}";

                if (string.IsNullOrWhiteSpace(userSession.NickName))
                {
                    return CreateNickNameUrl(currentUrl);
                }

                if (!userSession.IsLogged || userSession.IsPasswordExpired)
                {
                    return ForceChangePasswordUrl(currentUrl);
                }

                if (userSession.IsForcedSecurityCode)
                {
                    return ForceChangeSecurityCodeUrl(currentUrl, userSession.SecurityCode);
                }
            }

            return null;
        }

        private static string CreateNickNameUrl(string currentUrl)
            => currentUrl.Equals("NickName/Index", StringComparison.OrdinalIgnoreCase)
            ? null
            : "~/NickName/Index";

        private static string ForceChangePasswordUrl(string currentUrl)
            => currentUrl.Equals("Password/ForceChangePassword", StringComparison.OrdinalIgnoreCase)
            ? null
            : "~/Password/ForceChangePassword";

        private static string ForceChangeSecurityCodeUrl(string currentUrl, string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return currentUrl.Equals("SecurityCode/ForceChangeFirstLogin", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : "~/SecurityCode/ForceChangeFirstLogin";
            }

            return currentUrl.Equals("SecurityCode/ForceChangeSecurityCode", StringComparison.OrdinalIgnoreCase)
                ? null
                : "~/SecurityCode/ForceChangeSecurityCode";
        }
    }
}
