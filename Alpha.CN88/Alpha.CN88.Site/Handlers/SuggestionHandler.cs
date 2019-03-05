namespace Alpha.CN88.Site.Handlers
{
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Requests;
    using MediatR;
    using Params;
    using Sunplus.Agent.Model.Entities;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class SuggestionHandler : IAsyncRequestHandler<SuggestionCommand, SuggestionViewModel>
    {
        private readonly IUserSession userSession;
        private readonly SuggestionService suggestionService;

        public SuggestionHandler(IUserSession userSession, SuggestionService suggestionService)
        {
            this.userSession = userSession;
            this.suggestionService = suggestionService;
        }

        public async Task<SuggestionViewModel> Handle(SuggestionCommand message)
        { 
            var result = await suggestionService.SearchUser(new SuggestionParam
            (
                userSession.CustId,
                userSession.IsInternal,
                message.Query,
                userSession.IsSubAcc,
                message.ExcludeMember,
                message.DirectDownline,
                message.IsAccInfo
            ));

            return new SuggestionViewModel(message.Query, result.Select(i => new string[] { i.Username, i.Firstname, i.Lastname }));
        }        
    }
}