namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Alpha.Web.Helpers;
    using CN88.Helpers;
    using global::CN88.Auth.AppResources;
    using MediatR;
    using Requests.Authentication;
    using Sunplus.Agent.Utilities;

    public class AuthController : Controller
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            Session.Abandon();

            string beforeLoginUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl;
            string baseUrl = Sunplus.Agent.Model.BL.Definitions.AppSettings.BaseUrl;

            if (beforeLoginUrl != baseUrl)
            {
                return Redirect(beforeLoginUrl);
            }

            return new EmptyResult();
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Login(LoginQuery message)
            => ProcessLogin(message);

        public ActionResult AdminLogin(AdminLoginQuery message)
            => ProcessLogin(message);

        public ActionResult Logout(LogoutQuery message)
        {
            mediator.Send(message);
            Session.RemoveAll();

            if (!Session.IsCookieless)
            {
                ClearCookies();
                FormsAuthentication.SignOut();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Kickout(string reason, string culture)
        {
            var cultureInfo = GetCultureInfo(culture);
            var resourceManager = new ResourceManager(typeof(AuthResource));

            var message = string.IsNullOrWhiteSpace(reason)
                ? string.Empty
                : resourceManager.GetString(reason, cultureInfo);

            message = string.IsNullOrWhiteSpace(message)
                ? resourceManager.GetString("AccessDeny", cultureInfo)
                : message;

            return View(message as object);
        }

        private ActionResult ProcessLogin(LoginQuery message)
        {
            var isAdminLogin = message is AdminLoginQuery;

            if (message.IsInvalid())
            {
                if (isAdminLogin)
                {
                    return new EmptyResult();
                }

                return RedirectToAction(nameof(Index));
            }

            message.IBCKeyPath = GetIBCKeyPath("ibckey");
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                if (isAdminLogin)
                {
                    return new EmptyResult();
                }

                return RedirectToAction(nameof(Index));
            }

            var userName = string.IsNullOrWhiteSpace(result.SubAccName) ? result.CustName : result.SubAccName;

            if (!isAdminLogin)
            {
                var cookie = new HttpCookie("detecas-username") { Value = userName };
                Response.Cookies.Add(cookie);

                Web.Validators.SessionValidator.AddSessionVerificationToken();
            }

            var cookieNameOfCurrentPage = CookieHelper.GetCookieNameOfCurrentPage(result.CustId, result.SubAccId, Encryption.PearsonEncode);
            Request.Cookies.Remove(cookieNameOfCurrentPage);

            FormsAuthentication.RedirectFromLoginPage(userName, false, FormsAuthentication.FormsCookiePath);

            if (!string.IsNullOrWhiteSpace(result.AfterLogInSite) && !string.IsNullOrWhiteSpace(result.AfterLogInQuery))
            {
                Response.Cookies.Set(new HttpCookie(cookieNameOfCurrentPage, AppPathHelper.GetAppPath(result.AfterLogInSite, result.AfterLogInQuery)));
            }

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        private string GetIBCKeyPath(string fileName)
            => Server.MapPath($"~/App_Data/IbcKeys/{fileName}.snk");

        private void ClearCookies()
        {
            var yesterday = DateTime.Now.AddDays(-1);

            var requestCookies = Request.Cookies;
            var requestCookiesKeys = requestCookies.AllKeys;
            var responseCookies = Response.Cookies;

            foreach (var cookie in requestCookiesKeys)
            {
                requestCookies[cookie].Expires = yesterday;
            }

            foreach (string cookieName in requestCookiesKeys)
            {
                var cookie = requestCookies.Get(cookieName);

                if (cookie != null)
                {
                    cookie = new HttpCookie(cookieName);
                    cookie.Expires = yesterday;
                    responseCookies.Add(cookie);
                }
            }
        }

        private static CultureInfo GetCultureInfo(string cultureName)
        {
            var validCultureName = new[] { "en-US", "zh-Hans", "zh-TW" };
            var culture = !string.IsNullOrWhiteSpace(cultureName)
                ? validCultureName.FirstOrDefault(c => c.Equals(cultureName, StringComparison.OrdinalIgnoreCase))
                : null;

            return new CultureInfo(culture ?? validCultureName[0]);
        }
    }
}