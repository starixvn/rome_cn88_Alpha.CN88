namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;

    public class HomeController : SiteBaseController
    {
        public HomeController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index() => View();
    }
}