namespace Alpha.CN88.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Sunplus.Agent.Model.Entities;

    public class LanguageController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public LanguageController(IMediator mediator, IUserSession userSession) : base(mediator)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException(nameof(userSession));
            }

            this.userSession = userSession;
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            var viewModel = mediator.Send(new AllLanguagesQuery(userSession.Language));

            return PartialView(viewModel);
        }

        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageCommand(userSession, languageCode));

            return Json(viewModel);
        }
    }
}