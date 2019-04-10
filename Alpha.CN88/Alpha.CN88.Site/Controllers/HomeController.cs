namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.Home;
    using System.Web.SessionState;

    public class HomeController : SiteBaseController
    {
        public HomeController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult Index() => View();

        [HttpGet]
        [ChildActionOnly]
        public ActionResult GeneralInformation(HomeQuery message)
            => PartialView(mediator.Send(message));
    }
}