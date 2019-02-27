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

        public ActionResult Index(IndexQuery message)
        {
            if (message.IsForcedSecurityCode)
            {
                return string.IsNullOrWhiteSpace(message.SecurityCode)
                    ? RedirectToAction(nameof(ChangeSecurityCodeFirstLogin))
                    : RedirectToAction(nameof(ForceChangeSecurityCode));
            }

            return RedirectToAction("Index", "Default");
        }

        public ActionResult ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeQuery message)
        {
            if (message.IsForcedSecurityCode && string.IsNullOrWhiteSpace(message.SecurityCode))
            {
                return View(mediator.Send(message));
            }

            return RedirectToAction("Index", "Default");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand message)
        {
            var result = message.IsValid()
                && message.IsForcedSecurityCode
                && string.IsNullOrWhiteSpace(message.SecurityCodeInSession)
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            if (result.IsFail)
            {
                return View(new ForceChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
            }

            return RedirectToAction("Index", "Default");
        }

        public ActionResult ForceChangeSecurityCode(ForceChangeSecurityCodeQuery message)
        {
            if (message.IsForcedSecurityCode && !string.IsNullOrWhiteSpace(message.SecurityCode))
            {
                return View(mediator.Send(message));
            }

            return RedirectToAction("Index", "Default");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangeSecurityCode(ForceChangeSecurityCodeCommand message)
        {
            var result = message.IsValid()
               && message.IsForcedSecurityCode
               && !string.IsNullOrWhiteSpace(message.SecurityCodeInSession)
               ? mediator.Send(message)
               : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            if (result.IsFail)
            {
                return View(new ForceChangeSecurityCodeQuery(message.GetViewer()) { Result = result });
            }

            return RedirectToAction("Index", "Default");
        }
    }
}