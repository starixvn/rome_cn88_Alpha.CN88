namespace CN88.Web.Filters
{
    using System;
    using System.Collections.Generic;
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

            if (IsValidSessionAndUrl(filterContext))
            {
                var redirectUrl = GetAfterLoginFlowUrl();

                if (redirectUrl != null)
                {
                    filterContext.Result = new RedirectResult(MicroSites.MainUrl(redirectUrl));
                }
            }
        }

        private bool IsValidSessionAndUrl(ActionExecutingContext filterContext)
            => filterContext.HttpContext?.Session != null
            && userSession.UserId > 0
            && !userSession.IsInternal
            && !IsIgnoreUrl(filterContext);

        private static bool IsIgnoreUrl(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            return IgnoreURLs.ContainsKey($"{controllerName.ToLowerInvariant()}/{actionName.ToLowerInvariant()}");
        }

        private static Dictionary<string, bool> IgnoreURLs
            => new Dictionary<string, bool>
            {
                ["auth/index"] = true,
                ["auth/login"] = true,
                ["auth/adminlogin"] = true,
                ["auth/logout"] = true,
                ["nickname/index"] = true,
                ["password/forcechangepassword"] = true,
                ["securitycode/forcechangefirstlogin"] = true,
                ["securitycode/forcechangesecuritycode"] = true
            };

        private string GetAfterLoginFlowUrl()
        {
            if (string.IsNullOrWhiteSpace(userSession.NickName))
            {
                return "NickName/Index";
            }

            if (!userSession.IsLogged || userSession.IsPasswordExpired)
            {
                return "Password/ForceChangePassword";
            }

            if (userSession.IsForcedSecurityCode)
            {
                return string.IsNullOrWhiteSpace(userSession.SecurityCode)
                    ? "SecurityCode/ForceChangeFirstLogin"
                    : "SecurityCode/ForceChangeSecurityCode";
            }

            return null;
        }
    }
}
