namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using global::CN88.Auth.Models;
    using MediatR;
    using Requests.SecurityCode;

    public class SecurityCodeController : SiteBaseController
    {
        public SecurityCodeController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeQuery message)
            => message.IsForcedSecurityCode && string.IsNullOrWhiteSpace(message.SecurityCode)
                ? View(message)
                : ToDefaultPage();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand message)
        {
            var result = message.IsValid()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new ForceChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
        }

        public ActionResult ForceChangeSecurityCode(ForceChangeSecurityCodeQuery message)
            => message.IsForcedSecurityCode && !string.IsNullOrWhiteSpace(message.SecurityCode)
                ? View(message)
                : ToDefaultPage();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangeSecurityCode(ForceChangeSecurityCodeCommand message)
        {
            var result = message.IsValid()
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new ForceChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
        }
    }
}