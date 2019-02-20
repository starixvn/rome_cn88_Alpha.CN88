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
            Result result = Result.CreateSuccessResult();
            var onlineUserId = user.OnlineUserId;

            if (onId == 0 || onId != onlineUserId)
            {
                result = Result.CreateErrorResult(1, AuthResource.AccessDeny);
            }
            else
            {
                if (authenticationService.IsKickedOut(user.Id, onlineUserId))
                {
                    result = Result.CreateErrorResult(3, AuthResource.SessionKickedOut);
                }
                else if (!user.IsInternal && !authenticationService.IsActiveCustomer(user.Id, user.SubAccountId))
                {
                    result = Result.CreateErrorResult(2, AuthResource.CustomerClosed);
                }
            }

            if (!result.IsSuccess)
            {
                authenticationService.Logout(
                    new LogoutParam(onlineUserId, user.Name, user.SubUserName, user.NickName, user.LevelId, user.ServerId));

                Session.RemoveAll();
                Session.Abandon();

                if (!Session.IsCookieless)
                {
                    FormsAuthentication.SignOut();
                }
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}