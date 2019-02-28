namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using global::CN88.Auth.Models;
    using MediatR;
    using Requests.Password;

    public class PasswordController : SiteBaseController
    {
        public PasswordController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ForceChangePassword(ChangePasswordQuery message)
            => message.IsValid() ? View(message) : ToDefaultPage();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangePassword(ChangePasswordCommand message)
        {
            var result = message.IsValidForceChangePassword()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new ChangePasswordQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ChangePassword(ChangePasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordCommand message)
        {
            var result = message.IsValidChangePassword()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ChangePasswordQuery(message.GetViewer()) { Result = result });
        }

        //[CheckBelongingFilter]
        public ActionResult ResetDownlinePassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public ActionResult ResetDownlinePassword(ResetDownlinePasswordCommand message)
        {
            var result = message.IsValid()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ResetPasswordQuery(message.GetViewer())
            {
                Result = result
            });
        }

        //[CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand message)
        {
            var result = message.IsValid()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ResetPasswordQuery(message.GetViewer())
            {
                Result = result
            });
        }
    }
}