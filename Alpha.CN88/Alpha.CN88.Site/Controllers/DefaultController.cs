namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.Default;
    using Sunplus.Agent.Model.Entities;

    public class DefaultController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public DefaultController(
            IMediator mediator,
            IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index(IndexQuery message) => View(mediator.Send(message));

        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(userSession.RoleId, Request.UserAgent));

            return PartialView(viewModel);
        }
    }
}