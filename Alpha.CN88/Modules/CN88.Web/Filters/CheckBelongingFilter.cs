namespace CN88.Web.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AppResources;
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
                        KickoutUser(filterContext, WebResource.AccessDeny);
                        return;
                    }

                    if (!authorizationService.IsAuthorized(belongingContext))
                    {
                        KickoutUser(filterContext, WebResource.AccessDeny);
                        return;
                    }
                }

                if (belongingContext.EnableKickout)
                {
                    if (!authorizationService.IsCustomerActive(belongingContext))
                    {
                        KickoutUser(filterContext, WebResource.CustomerClosed);
                        return;
                    }

                    if (!AppSettings.DisabledCheckDuplicationLogin
                        && authorizationService.IsKickedOut(belongingContext))
                    {
                        KickoutUser(filterContext, WebResource.SessionKickedOut);
                    }
                }
            }
        }

        private static string GetCurrentUrl(AuthorizationContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;

            return $"{actionDescriptor.ControllerDescriptor.ControllerName}/{actionDescriptor.ActionName}";
        }

        private static void KickoutUser(AuthorizationContext filterContext, string message)
        {
            var httpContext = filterContext.HttpContext;

            var kickoutPageAbsoluteUrl = VirtualPathUtility.ToAbsolute(AuthorizationSection.Instance.KickoutUrl);
            var currentPageAbsoluteUrl = httpContext.Request.Url.AbsolutePath;

            if (kickoutPageAbsoluteUrl.Equals(currentPageAbsoluteUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (httpContext.Request.IsAjaxRequest())
            {
                var response = httpContext.Response;
                response.Clear();
                response.Write("{ \"message\": \"" + message + "\" }");
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.MainUrl($"{AuthorizationSection.Instance.KickoutUrl}?reason={message}"));
            }
        }
    }
}
