using MediatR;
using System.Data;
using System.Threading.Tasks;

namespace Alpha.CN88.Site.Requests
{
    public class SuggestionCommand : IRequest<Task<DataSet>>
    {
        public string Query { get; set; }

        public bool IsSubAccount { get; set; }

        public bool ExcludeMember { get; set; }

        public bool DirectDownline { get; set; }

        public bool IsAccInfo { get; set; }
    }
}