namespace Alpha.CN88.Site.Requests
{
    using MediatR;
    using System.Threading.Tasks;
    using ViewModels;

    public class SuggestionCommand : IRequest<Task<SuggestionViewModel>>
    {
        public string Query { get; set; }
        
        public bool ExcludeMember { get; set; }

        public bool DirectDownline { get; set; }

        public bool IsAccInfo { get; set; }
    }
}