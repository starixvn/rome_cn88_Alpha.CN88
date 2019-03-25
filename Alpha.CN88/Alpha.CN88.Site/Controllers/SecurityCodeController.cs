namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using global::CN88.Auth.Models;
    using MediatR;
    using Requests.SecurityCode;
    using Web.Filters;

    public class SecurityCodeController : SiteBaseController
    {
        public SecurityCodeController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ForceChangeFirstLogin(ResetSecurityCodeQuery message)
            => message.IsValidForceChangeFirstLogin() ? View(message) : ToDefaultPage();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangeFirstLogin(ResetSecurityCodeCommand message)
        {
            var result = message.IsValidForceChangeFirstLogin()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new ResetSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ForceChangeSecurityCode(ChangeSecurityCodeQuery message)
            => message.IsValidForceChangeSecurityCode() ? View(message) : ToDefaultPage();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangeSecurityCode(ChangeSecurityCodeCommand message)
        {
            var result = message.IsValidForceChangeSecurityCode()
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new ChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ChangeSecurityCode(ChangeSecurityCodeQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangeSecurityCode(ChangeSecurityCodeCommand message)
        {
            var result = message.IsValidChangeSecurityCode()
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ResetSecurityCode(ResetSecurityCodeQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetSecurityCode(ResetSecurityCodeCommand message)
        {
            var result = message.IsValidResetSecurityCode()
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ResetSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ResetSubAccountSecurityCode(ResetSecurityCodeQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetSubAccountSecurityCode(ResetSubAccountSecurityCodeCommand message)
        {
            var result = message.IsValid()
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return View(new ResetSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult VerifySecurityCode(VerifySecurityCodeQuery message)
            => View(message);

        [HttpPost, AntiXsrfFilter]
        public ActionResult VerifySecurityCode(VerifySecurityCodeCommand message)
            => JsonData(message, JsonRequestBehavior.DenyGet);
    }
}