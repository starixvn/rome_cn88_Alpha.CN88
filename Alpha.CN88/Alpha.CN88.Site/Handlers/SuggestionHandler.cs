using Alpha.CN88.Services;
using Alpha.CN88.Site.Requests;
using MediatR;
using Sunplus.Agent.Model.Entities;
using System.Data;
using System.Threading.Tasks;

namespace Alpha.CN88.Site.Handlers
{
    public class SuggestionHandler : IRequestHandler<SuggestionCommand, Task<DataSet>>
    {
        private readonly IUserSession userSession;
        private readonly SuggestionService suggestionService;

        public SuggestionHandler(IUserSession userSession, SuggestionService suggestionService)
        {
            this.userSession = userSession;
            this.suggestionService = suggestionService;
        }

        public Task<DataSet> Handle(SuggestionCommand message)
            => suggestionService.SearchUser
                    (
                        userSession.UserId,
                        message.Query,
                        message.IsSubAccount,
                        message.ExcludeMember,
                        message.DirectDownline,
                        message.IsAccInfo,
                        userSession.IsInternal
                    );
    }
}