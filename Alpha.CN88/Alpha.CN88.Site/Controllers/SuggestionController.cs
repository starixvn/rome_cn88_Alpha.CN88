namespace Alpha.CN88.Site.Controllers
{
    using Alpha.CN88.Site.Requests;
    using CN88.Models;
    using MediatR;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SuggestionController : SiteBaseController
    {
        private readonly IUser user;

        public SuggestionController(IUser user, IMediator mediator) : base(mediator)
        {
            this.user = user;
        }

        public async Task<ActionResult> Index(string query, bool isDir = false, bool isMember = false)
            => Content(JsonConvert.SerializeObject(await mediator.SendAsync(new SuggestionCommand(user, query, isDir, isMember))));
    }
}