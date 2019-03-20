namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Models;
    using MediatR;
    using Requests;
    using Requests.Default;

    [SessionState(SessionStateBehavior.Required)]
    public class DefaultController : SiteBaseController
    {
        private const string UnSupportedBrowsers = "UnsupportedBrowsers";

        private readonly IUser user;

        public DefaultController(IMediator mediator, IUser user)
            : base(mediator)
        {
            this.user = user;
        }

        [HttpGet]
        public ActionResult Index(IndexQuery message)
        {
            var browser = mediator.Send(new BrowserQuery(user));

            if (browser.IsUnsupportedBrowser)
            {
                return View(UnSupportedBrowsers, browser);
            }

            user.ClearClientData = false;

            return View(mediator.Send(message));
        }

        public ActionResult MenuBar()
        {
            var viewModel = mediator.Send(new MenuBarQuery(user.LevelId, Request.UserAgent));

            return PartialView(viewModel);
        }
    }
}