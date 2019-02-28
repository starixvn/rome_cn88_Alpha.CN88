namespace Alpha.CN88.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using CN88.Models;
    using MediatR;
    using Requests;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
            => View(mediator.Send(message));

        protected ActionResult JsonData<TResponse>(
            IRequest<TResponse> message,
            JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if (!(message as BaseQuery).IsValid())
            {
                return Json(Result.CreateForbiddenResult());
            }

            return Json(mediator.Send(message), jsonRequestBehavior);
        }

        protected ActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}