namespace CN88.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Helpers;
    using Services;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CheckSubAccBelongingFilterAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IUserSession userSession = new UserSessionExt();
            var actorId = userSession.CustId;

            if (actorId == 0 || filterContext.Result is HttpUnauthorizedResult)
            {
                UnAuthorizationHelper.UnauthorizeOrLogin(filterContext);
                return;
            }

            if (!userSession.IsInternal)
            {
                var authorizationService = new AuthorizationServiceImpl();

                if (!authorizationService.IsSubAccBelonging(filterContext.HttpContext.Request, actorId))
                {
                    UnAuthorizationHelper.UnauthorizeOrKickout(filterContext, "AccessDeny");
                }
            }
        }
    }
}
