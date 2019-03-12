namespace CN88.Web.Controllers
{
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Configurations;
    using Helpers;
    using Sunplus.Agent.Model.Entities;

    public class CN88BaseController : Controller
    {
        public const string English = "en-US";
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(English);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var user = filterContext.HttpContext?.User;
            var isAuthenticated = AppSettings.IsIntegrationTest ||
                (user != null && user.Identity.IsAuthenticated && GetUserSession().UserId > 0);

            if (!isAuthenticated)
            {
                UnAuthentication(filterContext);
            }
        }

        protected void UnAuthentication(AuthorizationContext filterContext)
            => UnAuthorizationHelper.ProcessUnAuthorization(filterContext);

        protected override JsonResult Json(
            object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
            => new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };

        protected override void Initialize(RequestContext requestContext)
        {
            var userSession = GetUserSession();
            var cultureName = userSession.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = English;
                userSession.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }

        private static IUserSession GetUserSession()
            => UserSession.Current;
    }
}
