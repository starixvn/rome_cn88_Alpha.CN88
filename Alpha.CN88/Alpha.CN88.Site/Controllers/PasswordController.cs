namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
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
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangePassword(ChangePasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ChangePasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = mediator.Send(message);

                if (result.IsFail)
                {
                    return View(new ChangePasswordQuery(result));
                }

                return RedirectToAction("Index", "Default");
            }
        }

        //[CheckBelongingFilter]
        public ActionResult ResetMainAccountPassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public async Task<ActionResult> ResetMainAccountPassword(ResetMainAccountPasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ResetPasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = await mediator.SendAsync(message);

                if (result.IsFail)
                {
                    return View(new ResetPasswordQuery(result));
                }

                return RedirectToAction("Index", "Default");
            }
        }

        //[CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public async Task<ActionResult> ResetSubAccountPassword(ResetSubAccountPasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ResetPasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = await mediator.SendAsync(message);

                if (result.IsFail)
                {
                    return View(new ResetPasswordQuery(result));
                }

                return RedirectToAction("Index", "Default");
            }
        }

        //[HttpPost, AntiXsrfFilter]
        //public ActionResult ChangePassword(ChangePasswordCommand command)
        //    => Json(command.IsValid()
        //        ? mediator.Send(command)
        //        : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden), JsonRequestBehavior.DenyGet);
    }
}