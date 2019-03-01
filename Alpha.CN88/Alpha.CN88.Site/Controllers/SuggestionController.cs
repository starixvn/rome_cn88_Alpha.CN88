namespace Alpha.CN88.Site.Controllers
{
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class SuggestionController : SiteBaseController
    {
        public SuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(SuggestionCommand query)
        {
            var result = await mediator.Send(query);

            return Json(result);
        }
    }
}