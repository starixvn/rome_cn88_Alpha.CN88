namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using CN88.Models;
    using Fanex.Logging.Extensions.Web;
    using global::CN88.Auth.AppResources;
    using global::CN88.Auth.Constants;
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.Authentication;
    using global::CN88.Auth.Services;
    using Web;

    public class ReCheckinController : BaseController
    {
        private readonly IUser user;
        private readonly AuthenticationService authenticationService;

        public ReCheckinController(IUser user, AuthenticationService authenticationService)
        {
            this.user = user;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public ActionResult Index(int onId)
        {
            var result = authenticationService.IsDisabledCheckDuplicationLogin
                ? new SecurityResult()
                : ProcessReCheckin(onId);

            if (!result.IsSuccess)
            {
                LogoutAndClearSession();
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        private SecurityResult ProcessReCheckin(int onlineUserId)
        {
            if (onlineUserId == 0 || onlineUserId != user.OnlineUserId)
            {
                return SecurityResult.CreateErrorResult(AuthResource.AccessDeny, 1);
            }

            if (authenticationService.IsKickedOut(onlineUserId))
            {
                return SecurityResult.CreateErrorResult(AuthResource.SessionKickedOut, 3);
            }

            if (!user.IsInternal && !authenticationService.IsActiveCustomer(user.Id, user.SubAccountId))
            {
                return SecurityResult.CreateErrorResult(AuthResource.CustomerClosed, 2);
            }

            return new SecurityResult();
        }

        private void LogoutAndClearSession()
        {
            try
            {
                authenticationService.Logout(new LogoutParam(
                    user.OnlineUserId,
                    user.Name,
                    user.SubUserName,
                    user.NickName,
                    user.LevelId,
                    user.ServerId,
                    LogoutReason.Kickedout));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            Session.RemoveAll();
            Session.Abandon();

            if (!Session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}