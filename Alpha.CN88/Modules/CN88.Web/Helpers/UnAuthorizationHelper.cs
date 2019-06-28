namespace CN88.Web.Helpers
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using AppResources;
    using Configurations;
    using Sunplus.Agent.Model.Entities;

    internal static class UnAuthorizationHelper
    {
        internal static void UnauthorizeOrLogin(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UnAuthorizationAjaxResult(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.MainUrl("~/auth/index"));
            }
        }

        internal static void UnauthorizeOrKickout(AuthorizationContext filterContext, string message)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UnAuthorizationAjaxResult(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult(
                    MicroSites.MainUrl($"{AuthorizationSection.Instance.KickoutUrl}?reason={message}&culture={UserSession.Current.Language}"));
            }

            ClearSession(filterContext.HttpContext.Session);
        }

        private static void UnAuthorizationAjaxResult(AuthorizationContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Clear();
            response.Write("{ \"message\": \"" + WebResource.AccessDeny + "\" }");

            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

            response.TrySkipIisCustomErrors = true;
            response.End();            
        }

        private static void ClearSession(HttpSessionStateBase session)
        {
            session.RemoveAll();
            session.Abandon();

            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
