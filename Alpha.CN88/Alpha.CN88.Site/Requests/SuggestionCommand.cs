namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SuggestionCommand : IRequest<Task<IEnumerable<SuggestionModel>>>
    {
        public string Query { get; set; }
        
        public bool ExcludeMember { get; set; }

        public bool DirectDownline { get; set; }

        public bool IsAccInfo { get; set; }
    }
}