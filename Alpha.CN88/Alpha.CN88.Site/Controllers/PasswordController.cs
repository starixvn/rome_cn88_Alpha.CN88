namespace Alpha.CN88.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Password;
    using Web.Filters;

    public class PasswordController : SiteBaseController
    {
        public PasswordController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult ForceChangePassword(ForceChangePasswordQuery message)
            => View(message);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand command)
            => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
            => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
            => Json(await mediator.SendAsync(command), JsonRequestBehavior.DenyGet);
    }
}