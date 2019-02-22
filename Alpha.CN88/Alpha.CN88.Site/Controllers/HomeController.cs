namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.Home;

    public class HomeController : SiteBaseController
    {
        public HomeController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index() => View();

        [HttpGet]
        [ChildActionOnly]
        public ActionResult AccountOverview(AccountOverviewQuery message)
            => PartialView(mediator.Send(message));

        [HttpGet]
        [ChildActionOnly]
        public ActionResult StatusSummary(StatusSummaryQuery message)
            => PartialView(mediator.Send(message));

        [HttpGet]
        public ActionResult ImportantAnnouncement(ImportantAnnouncementQuery message)
            => PartialView(mediator.Send(message));

        [HttpGet]
        public ActionResult GeneralAnnouncement(GeneralAnnouncementQuery message)
            => PartialView(mediator.Send(message));
    }
}