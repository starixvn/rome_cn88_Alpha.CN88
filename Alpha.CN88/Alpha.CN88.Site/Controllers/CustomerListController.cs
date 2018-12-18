namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Models;
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
            => View(mediator.Send(message));

        [HttpGet]
        public JsonResult GetCustomerList(CustomerListQuery message)
            => Json(mediator.Send(message), JsonRequestBehavior.AllowGet);

        //[AntiXsrfFilter]
        [HttpPost]
        public JsonResult UpdateClosed(UpdateClosedCommand command)
        {
            return Json(Result.CreateErrorResult(-101, "A problem has been occurred while submitting your data"));
        }

        [HttpPost]
        public JsonResult UpdateSuspended(UpdateSuspendedCommand command)
        {
            return Json(Result.CreateErrorResult(-101, "A problem has been occurred while submitting your data"));
        }

        [HttpPost]
        public JsonResult UpdateCredit()
        {
            return Json(Result.CreateErrorResult(-101, "A problem has been occurred while submitting your data"));
        }
    }
}