namespace Alpha.CN88.Site.Handlers
{
    using Alpha.CN88.Models;
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using Params;
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SuggestionHandler : IRequestHandler<SuggestionCommand, Task<IEnumerable<SuggestionModel>>>
    {
        private readonly IUserSession userSession;
        private readonly SuggestionService suggestionService;

        public SuggestionHandler(IUserSession userSession, SuggestionService suggestionService)
        {
            this.userSession = userSession;
            this.suggestionService = suggestionService;
        }

        public Task<IEnumerable<SuggestionModel>> Handle(SuggestionCommand message)
            => suggestionService.SearchUser(new SuggestionParam
            (
                userSession.CustId,
                userSession.IsInternal,
                message.Query,
                userSession.IsSubAcc,
                message.ExcludeMember,
                message.DirectDownline,
                message.IsAccInfo
            ));
    }
}