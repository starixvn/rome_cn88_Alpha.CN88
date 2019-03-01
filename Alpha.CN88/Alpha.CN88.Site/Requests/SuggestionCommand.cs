namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SuggestionCommand : IRequest<Task<IEnumerable<SuggestionModel>>>
    {
        public SuggestionCommand(string query, bool excludeMember, bool directDownline, bool isAccInfo)
        {
            Query = query;
            ExcludeMember = excludeMember;
            DirectDownline = directDownline;
            IsAccInfo = isAccInfo;
        }

        public string Query { get; }
        
        public bool ExcludeMember { get; }

        public bool DirectDownline { get; }

        public bool IsAccInfo { get; }
    }
}