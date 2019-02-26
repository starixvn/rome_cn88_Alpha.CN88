namespace Alpha.CN88.Site.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using AppResources;
    using global::CN88.Auth.Models;
    using MediatR;
    using Requests.NickName;
    using ViewModels.NickName;

    public class NickNameController : SiteBaseController
    {
        public NickNameController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult Index(CreateNickNameViewModel viewModel)
            => View(viewModel);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CreateNickNameCommand message)
        {
            if (!message.IsValid())
            {
                return View(new CreateNickNameViewModel(
                    SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden)));
            }
            else
            {
                var result = mediator.Send(message);

                if (result.IsFail)
                {
                    return View(new CreateNickNameViewModel(result));
                }

                return RedirectToAction("Index", "Default");
            }
        }
    }
}