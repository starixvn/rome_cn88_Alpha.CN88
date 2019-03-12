namespace CN88.Web.Helpers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using Configurations;

    internal static class UnAuthorizationHelper
    {
        internal static void ProcessUnAuthorization(AuthorizationContext filterContext, string message = null)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UnAuthorizationAjaxResult(filterContext, message ?? WebResource.AccessDeny);
            }
            else
            {
                filterContext.Result = message == null ? Auth() : Kickout(message);
            }
        }

        private static void UnAuthorizationAjaxResult(AuthorizationContext filterContext, string message)
        {
            var response = filterContext.HttpContext.Response;
            response.Clear();
            response.Write("{ \"message\": \"" + message + "\" }");

            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

            response.TrySkipIisCustomErrors = true;
            response.End();
        }

        private static RedirectResult Auth()
            => new RedirectResult(MicroSites.MainUrl("~/auth/index"));

        private static RedirectResult Kickout(string message)
            => new RedirectResult(MicroSites.MainUrl($"{AuthorizationSection.Instance.KickoutUrl}?reason={message}"));
    }
}
