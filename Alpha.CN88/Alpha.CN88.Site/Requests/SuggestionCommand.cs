namespace Alpha.CN88.Site.Requests
{
    using MediatR;
    using ViewModels;

    public class SuggestionCommand : IAsyncRequest<SuggestionViewModel>
    {
        public string Query { get; set; }
        
        public bool ExcludeMember { get; set; }

        public bool DirectDownline { get; set; }

        public bool IsAccInfo { get; set; }
    }
}