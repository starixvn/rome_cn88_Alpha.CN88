namespace Alpha.CN88.Site.Controllers
{
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using Newtonsoft.Json;
    using System.Collections.Generic;
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
            var suggestions = new List<string[]>();
            foreach (var customer in result)
            {
                suggestions.Add(new string[] { customer.Username, customer.Firstname, customer.Lastname });
            }

            var viewModel = new SuggestionViewModel(command.Query, suggestions);

            return Content(JsonConvert.SerializeObject(viewModel));
        }
    }
}