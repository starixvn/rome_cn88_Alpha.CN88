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

        public ActionResult ChangePassword(ChangePasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ChangePasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = mediator.Send(message);

                return View(new ChangePasswordQuery(result));
            }
        }

        //[CheckBelongingFilter]
        public ActionResult ResetDownlinePassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public ActionResult ResetDownlinePassword(ResetDownlinePasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ResetPasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = mediator.Send(message);

                return View(new ResetPasswordQuery(result)
                {
                    CustId = message.CustId,
                    CustName = message.CustName
                });
            }
        }

        //[CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetPasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        //[CheckBelongingFilter]
        public ActionResult ResetSubAccountPassword(ResetSubAccountPasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ResetPasswordQuery(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = mediator.Send(message);

                return View(new ResetPasswordQuery(result)
                {
                    CustId = message.CustId,
                    CustName = message.CustName
                });
            }
        }
    }
}