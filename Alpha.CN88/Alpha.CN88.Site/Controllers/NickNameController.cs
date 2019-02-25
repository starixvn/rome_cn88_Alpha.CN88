namespace Alpha.CN88.Site.Controllers
{
    using System.Web.Mvc;
    using CN88.Models;
    using MediatR;
    using Requests.NickName;

    public class NickNameController : SiteBaseController
    {
        public NickNameController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult Index()
            => View();

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CreateNickNameCommand message)
        {
            if (!message.IsValid())
            {
                return View(Result.CreateForbiddenResult());
            }
            else
            {
                var result = mediator.Send(message);

                if (result.IsFail)
                {
                    return View(result);
                }

                return RedirectToAction("Index", "Default");
            }
        }
    }
}