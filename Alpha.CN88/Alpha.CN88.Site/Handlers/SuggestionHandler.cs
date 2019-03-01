namespace Alpha.CN88.Site.Handlers
{
    using Alpha.CN88.Criteria;
    using Alpha.CN88.Models;
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Requests;
    using MediatR;
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
            => suggestionService.SearchUser(new SuggestionCriteria
            {
                CustId = userSession.CustId,
                DirectDownline = message.DirectDownline,
                ExcludeMember = message.ExcludeMember,
                IsAccInfo = message.IsAccInfo,
                IsInternal = userSession.IsInternal,
                IsSubAccount = userSession.IsSubAcc,
                Username = message.Query
            });
    }
}