namespace Alpha.CN88.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using CN88.Models;
    using MediatR;
    using Requests.Password;

    public class PasswordController : SiteBaseController
    {
        public PasswordController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ForceChangePassword(ForceChangePasswordQuery message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForceChangePassword(ChangePasswordCommand message)
        {
            if (!message.IsValid())
            {
                return View(Result.CreateForbiddenResult());
            }
            else
            {
                var result = mediator.Send(message);

                if (result.IsFail)
                {
                    return View(result);
                }

                return RedirectToAction("Index", "Default");
            }
        }

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> ChangePassword(ChangePasswordCommand command)
        //    => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        //    => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        //    => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);
    }
}