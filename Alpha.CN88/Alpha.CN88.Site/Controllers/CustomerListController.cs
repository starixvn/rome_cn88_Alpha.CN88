namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.CustomerList;

    public class CustomerListController : SiteBaseController
    {
        public CustomerListController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index(IndexQuery message)
        {
            return View(mediator.Send(message));
        }
    }
}