namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using global::CN88.Auth.Models;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Model.Entities;
    using Web.Filters;

    public class PasswordController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public PasswordController(
            IMediator mediator,
            IUserSession userSession) : base(mediator)
        {
            this.userSession = userSession;
        }

        public ActionResult ForceChangePassword(ForceChangePasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangePassword(ChangePasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(new ForceChangePasswordQuery(message.Viewer)
                {
                    Result = SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)
                });
            }
            else
            {
                var result = mediator.Send(message);

                if (result.IsFail)
                {
                    return View(new ForceChangePasswordQuery(message.Viewer) { Result = result });
                }

                userSession.IsLogged = true;
                userSession.IsPasswordExpired = false;

                return RedirectToAction("Index", "Default");
            }
        }

        //[HttpPost, AntiXsrfFilter]
        //public ActionResult ChangePassword(ChangePasswordCommand command)
        //    => Json(command.IsValid()
        //        ? mediator.Send(command)
        //        : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden), JsonRequestBehavior.DenyGet);

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        //    => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        //    => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);
    }
}