namespace CN88.Web.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AppResources;
    using Sunplus.Agent.Model.Base;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class RestrictHeadlineFilter: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                var httpContext = filterContext.HttpContext;

                if (httpContext.Request.IsAjaxRequest())
                {
                    var response = httpContext.Response;
                    response.Clear();
                    response.Write("{ \"message\": \"" + WebResource.AccessDeny + "\" }");
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                    response.TrySkipIisCustomErrors = true;
                    response.End();
                }
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
            => base.AuthorizeCore(httpContext) && IsSMA;

        private static bool IsSMA
        {
            get
            {
                var roleId = new UserSessionExt().RoleId;

                return 0 < roleId && roleId < 5;
            }
        }
    }
}
