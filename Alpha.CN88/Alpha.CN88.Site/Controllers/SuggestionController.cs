using Alpha.CN88.Site.Requests;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.CN88.Site.Controllers
{
    public class SuggestionController : SiteBaseController
    {
        public SuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(SuggestionCommand query)
        {
            var result = await mediator.Send(query) ?? new DataSet();
            var table = result.Tables[0];

            return Json(table);
        }
    }
}