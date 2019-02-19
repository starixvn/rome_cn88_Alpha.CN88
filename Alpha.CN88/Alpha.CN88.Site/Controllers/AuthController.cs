namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using CN88.Helpers;
    using MediatR;
    using Requests.Authentication;
    using Sunplus.Agent.Utilities;

    public class AuthController : Controller
    {
        private const string DeviceIdSessionKey = "ProcessAfterLogin_Handler.DeviceIdSessionKey";

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
        {
            Session.RemoveAll();

            if (message.IsInvalid())
            {
                return RedirectToAction(nameof(Index));
            }

            message.IBCKeyPath = GetIBCKeyPath();
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            Session[DeviceIdSessionKey] = result.DeviceId;

            return SetCookiesAndRedirectToDefaultPage(result.CustId, result.CustName, result.SubAccId, result.SubAccName);
        }

        public ActionResult AdminLogin(AdminLoginQuery message)
        {
            Session.RemoveAll();

            if (message.IsInvalid())
            {
                return RedirectToAction(nameof(Index));
            }

            message.IBCKeyPath = GetIBCKeyPath();
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            Request.Cookies.Remove(CookieHelper.GetCookieNameOfCurrentPage(result.CustId, result.SubAccId, Encryption.PearsonEncode));
            FormsAuthentication.RedirectFromLoginPage(result.CustName, false);

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        public ActionResult Logout(LogoutQuery message)
        {
            Session.RemoveAll();
            mediator.Send(message);

            if (!Session.IsCookieless)
            {
                ClearCookies();
                FormsAuthentication.SignOut();
            }

            return RedirectToAction(nameof(Index));
        }

        private string GetIBCKeyPath()
            => Server.MapPath("~/App_Data/IbcKeys/ibckey.snk");

        private RedirectToRouteResult SetCookiesAndRedirectToDefaultPage(int custId, string custName, int subAccId, string subAccName)
        {
            Request.Cookies.Remove(CookieHelper.GetCookieNameOfCurrentPage(custId, subAccId, Encryption.PearsonEncode));

            var userName = string.IsNullOrWhiteSpace(subAccName) ? custName : subAccName;
            var cookie = new HttpCookie("detecas-username") { Value = userName };
            Response.Cookies.Add(cookie);

            Web.Validators.SessionValidator.AddSessionVerificationToken();

            //FormsAuthentication.SetAuthCookie(userName, false);
            FormsAuthentication.RedirectFromLoginPage(userName, false, FormsAuthentication.FormsCookiePath);

            return RedirectToAction("Index", "Default");
        }

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