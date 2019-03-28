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
            var currentUrl = $"{controllerName}/{actionDescriptor.ActionName}";

            if (IsSetNickName)
            {
                return CreateNickNameUrl(currentUrl);
            }

            if (IsForceChangePassword)
            {
                return ForceChangePasswordUrl(currentUrl);
            }

            if (userSession.IsForcedSecurityCode)
            {
                return ForceChangeSecurityCodeUrl(currentUrl, userSession.SecurityCode);
            }

            return null;
        }

        private bool IsSetNickName
            => !userSession.IsSubAcc && string.IsNullOrWhiteSpace(userSession.NickName);

        private bool IsForceChangePassword
            => !userSession.IsLogged || userSession.IsPasswordExpired;

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
