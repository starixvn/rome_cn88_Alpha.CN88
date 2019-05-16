namespace Alpha.CN88.Site.Handlers
{
    using MediatR;
    using Params;
    using Requests;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;

    public class SuggestionHandler : IAsyncRequestHandler<SuggestionCommand, SuggestionViewModel>
    {
        private readonly SuggestionService suggestionService;

        public SuggestionHandler(SuggestionService suggestionService)
        {
            this.suggestionService = suggestionService;
        }

        public async Task<SuggestionViewModel> Handle(SuggestionCommand message)
        {
            var result = await suggestionService.SearchUser(new SuggestionParam
            (
                message.UserId,
                message.CustId,
                message.IsInternal,
                message.Query,
                message.IsSubAcc,
                message.ExcludeMember,
                message.DirectDownline,
                message.IsAccInfo
            )).ConfigureAwait(false);

            return new SuggestionViewModel(message.Query, result.Select(i => new string[] { i.Username, i.Firstname, i.Lastname, i.Username2 }));
        }
    }
}