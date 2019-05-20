namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class HelpController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public HelpController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        public ActionResult NewUrl() => View();

        public ActionResult Troubleshooting()
            => View($"~/Views/Help/Troubleshooting/{userSession.Language.ToLowerInvariant()}/Index.cshtml");

        public ActionResult Cache()
            => View($"~/Views/Help/Troubleshooting/{userSession.Language.ToLowerInvariant()}/Cache.cshtml");

        public ActionResult LatestBrowser()
            => View($"~/Views/Help/Troubleshooting/{userSession.Language.ToLowerInvariant()}/LatestBrowser.cshtml");
    }
}