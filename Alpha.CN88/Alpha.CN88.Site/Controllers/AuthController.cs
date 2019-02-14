namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Helpers;
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
            return Content("Please login to start your session");
        }

        public ActionResult Login(LoginQuery message)
        {
            ClearSession();

            if (message.IsInvalid())
            {
                return RedirectToLoginPage();
            }

            message.IBCKeyPath = GetIBCKeyPath();
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                return RedirectToLoginPage();
            }

            //if (!Model.BL.Definitions.AppSettings.LocalTest)
            //{
            //    string[] domains = Model.BL.Definitions.AppSettings.CookielessDomainCollection;
            //    TplCore.TplSettings.SetCookielessDomains(domains);
            //}

            Session[DeviceIdSessionKey] = result.DeviceId;

            var userName = string.IsNullOrWhiteSpace(result.SubAccName) ? result.CustName : result.SubAccName;
            var cookie = new HttpCookie("detecas-username") { Value = userName };
            Response.Cookies.Add(cookie);

            return RedirectToDefaultPage(result.CustId, result.SubAccId, result.CustName);
        }

        public ActionResult AdminLogin(AdminLoginQuery message)
        {
            ClearSession();

            if (message.IsInvalid())
            {
                return RedirectToLoginPage();
            }

            message.IBCKeyPath = GetIBCKeyPath();
            var result = mediator.Send(message);

            if (!result.IsSuccess)
            {
                return RedirectToLoginPage();
            }

            //if (!Model.BL.Definitions.AppSettings.LocalTest)
            //{
            //    string[] domains = Model.BL.Definitions.AppSettings.CookielessDomainCollection;
            //    TplCore.TplSettings.SetCookielessDomains(domains);
            //}

            return RedirectToDefaultPage(result.CustId, result.SubAccId, result.CustName);
        }

        //public ActionResult Logout(LogoutQuery message)
        //{
        //    mediator.Send(message);
        //    ClearSession();

        //    if (Session.IsCookieless)
        //    {
        //        return RedirectToLoginPage();
        //    }
        //    else
        //    {
        //        FormsAuthentication.SignOut();

        //        ClearCookies();

        //        Response.Redirect("~/_Authorization/SignIn.aspx", true);
        //    }
        //}

        private string GetIBCKeyPath()
            => Server.MapPath("~/App_Data/IbcKeys/ibckey.snk");

        private RedirectResult RedirectToDefaultPage(int custId, int subAccId, string userName)
        {
            Request.Cookies.Remove(GetCookieCurrentURLKey(custId, subAccId));

            Web.Validators.SessionValidator.AddSessionVerificationToken();

            FormsAuthentication.RedirectFromLoginPage(userName, false, FormsAuthentication.FormsCookiePath);

            FormsAuthentication.SetAuthCookie(GetCookieCurrentURLKey(custId, subAccId), false);

            return Redirect(FormsAuthentication.DefaultUrl);
        }

        private string GetCookieCurrentURLKey(int custId, int subAccId)
            => Encryption.PearsonEncode($"{custId}_{subAccId}_URL");

        private RedirectResult RedirectToLoginPage()
            => Redirect(FormsAuthentication.LoginUrl);
        //=> Redirect(Sunplus.Agent.Model.BL.Definitions.AppSettings.BeforeLoginUrl);

        private void ClearSession()
        {
            // RemoveAll calls Clear internally.
            Session.RemoveAll();
            Session.Abandon();
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