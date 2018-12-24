namespace Alpha.CN88.Site.Controllers
{
    using Alpha.Web.Filters;
    using MediatR;
    using Requests.CustomerList;
    using System.Web.Mvc;

    public class CustomerListController : SiteBaseController
    {
        public CustomerListController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index(IndexQuery message)
            => View(mediator.Send(message));

        [HttpGet]
        public ActionResult Edit(int custId, string username)
            => View();

        [HttpGet]
        public JsonResult GetCustomerList(CustomerListQuery message)
            => Json(mediator.Send(message), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public ActionResult CreditPopup(CreditPopupQuery message)
           => View(mediator.Send(message));

        [HttpGet]
        public ActionResult StatusPopup(CustomerStatusPopupQuery message)
            => View(mediator.Send(message));

        [HttpPost, AntiXsrfFilter]
        public ActionResult UpdateCredit(CreditCommand command)
            => JsonData(command);

        [HttpPost, AntiXsrfFilter]
        public ActionResult CustomerStatus(CustomerStatusCommand command)
            => JsonData(command);
    }
}