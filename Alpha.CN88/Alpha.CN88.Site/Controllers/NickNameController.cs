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
            => viewModel.IsInternal ? ToDefaultPage() : View(viewModel);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CreateNickNameCommand message)
        {
            var result = message.IsValid()
                ? mediator.Send(message)
                : SecurityResult.CreateErrorResult(Resources.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccess
                ? ToDefaultPage()
                : View(new CreateNickNameViewModel(message.GetViewer()) { Result = result });
        }
    }
}