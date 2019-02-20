namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using CN88.Models;
    using global::CN88.Auth.AppResources;
    using global::CN88.Auth.Models;
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
                ? Result.CreateSuccessResult()
                : ProcessReCheckin(onId);

            if (!result.IsSuccess)
            {
                LogoutAndClearSession();
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        private Result ProcessReCheckin(int onlineUserId)
        {
            if (onlineUserId == 0 || onlineUserId != user.OnlineUserId)
            {
                return Result.CreateErrorResult(1, AuthResource.AccessDeny);
            }

            if (authenticationService.IsKickedOut(user.Id, onlineUserId))
            {
                return Result.CreateErrorResult(3, AuthResource.SessionKickedOut);
            }

            if (!user.IsInternal && !authenticationService.IsActiveCustomer(user.Id, user.SubAccountId))
            {
                return Result.CreateErrorResult(2, AuthResource.CustomerClosed);
            }

            return Result.CreateSuccessResult();
        }

        private void LogoutAndClearSession()
        {
            authenticationService.Logout(
                new LogoutParam(user.OnlineUserId, user.Name, user.SubUserName, user.NickName, user.LevelId, user.ServerId));

            Session.RemoveAll();
            Session.Abandon();

            if (!Session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}