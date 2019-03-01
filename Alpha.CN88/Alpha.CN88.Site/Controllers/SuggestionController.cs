namespace Alpha.CN88.Site.Controllers
{
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels;

    public class SuggestionController : SiteBaseController
    {
        public SuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(SuggestionCommand command)
        {
            var result = await mediator.Send(command);
            var viewModel = new SuggestionViewModel(command.Query, result.Select(customer => new string[] { customer.Username, customer.Firstname, customer.Lastname }));

            return Content(JsonConvert.SerializeObject(viewModel));
        }
    }
}