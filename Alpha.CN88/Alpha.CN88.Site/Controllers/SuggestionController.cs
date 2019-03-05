namespace Alpha.CN88.Site.Controllers
{
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class SuggestionController : SiteBaseController
    {
        public SuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(SuggestionCommand command)
            => Content(JsonConvert.SerializeObject(await mediator.SendAsync(command)));
    }
}