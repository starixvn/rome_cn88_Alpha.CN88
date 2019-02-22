namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Models;
    using MediatR;
    using Requests.SecurityCode;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SecurityCodeController : SiteBaseController
    {
        private readonly IUser user;

        public SecurityCodeController(IMediator mediator, IUser user)
            : base(mediator)
        {
            this.user = user;
        }

        public ActionResult Index()
        {
            if (user.IsForcedSecurityCode)
            {
                bool hasSC = !string.IsNullOrWhiteSpace(user.SecurityCode);

                return hasSC
                    ? RedirectToAction(nameof(ChangeSecurityCodeByForcing))
                    : RedirectToAction(nameof(ChangeSecurityCodeFirstLogin));
            }

            return RedirectToAction("Index", "Default");
        }

        public ActionResult ChangeSecurityCodeByForcing(ForceChangeSecurityCodeQuery message)
            => View(mediator.Send(message));

        public ActionResult ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeQuery message)
        {
            if (!string.IsNullOrWhiteSpace(user.SecurityCode))
            {
                return RedirectToAction("Index", "Default");
            }

            return View(mediator.Send(message));
        }
    }
}