namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Resources;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using CN88.Helpers;
    using CN88.Models;
    using global::CN88.Auth.AppResources;
    using global::CN88.Auth.Constants;
    using MediatR;
    using Models;
    using Requests.Authentication;
    using Sunplus.Agent.Model.Base;
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

            return Redirect(Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl);
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

        public ActionResult Kickout(string reason)
        {
            IUser user = new User(new UserSessionExt());
            var query = new LogoutQuery(user)
            {
                Reason = LogoutReason.Kickedout
            };

            mediator.Send(query);
            Session.RemoveAll();

            if (!Session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }

            var resourceManager = new ResourceManager(typeof(AuthResource));
            var message = resourceManager.GetString(reason);

            if (string.IsNullOrWhiteSpace(message))
            {
                message = AuthResource.AccessDeny;
            }

            return View(message as object);
        }

        private ActionResult ProcessLogin(LoginQuery message)
        {
            if (message.IsInvalid())
            {
                return RedirectToAction(nameof(Index));
            }

            message.IBCKeyPath = GetIBCKeyPath("ibckey");
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            var userName = string.IsNullOrWhiteSpace(result.SubAccName) ? result.CustName : result.SubAccName;
            var isAdminLogin = message is AdminLoginQuery;

            if (!isAdminLogin)
            {
                var cookie = new HttpCookie("detecas-username") { Value = userName };
                Response.Cookies.Add(cookie);

                Web.Validators.SessionValidator.AddSessionVerificationToken();
            }

            Request.Cookies.Remove(CookieHelper.GetCookieNameOfCurrentPage(result.CustId, result.SubAccId, Encryption.PearsonEncode));
            FormsAuthentication.RedirectFromLoginPage(userName, false, FormsAuthentication.FormsCookiePath);

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
    }
}