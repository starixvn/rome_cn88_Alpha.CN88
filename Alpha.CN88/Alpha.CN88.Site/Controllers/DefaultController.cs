namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using CN88.Models;
    using MediatR;
    using Requests.Default;

    public class DefaultController : SiteBaseController
    {
        private readonly IUser userSession;

        public DefaultController(IMediator mediator, IUser userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index(IndexQuery message) => View(mediator.Send(message));

        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(userSession.LevelId, Request.UserAgent));

            return PartialView(viewModel);
        }
    }
}