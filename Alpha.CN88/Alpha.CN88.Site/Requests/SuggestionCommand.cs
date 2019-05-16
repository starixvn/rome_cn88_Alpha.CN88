namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;
    using MediatR;
    using ViewModels;

    public class SuggestionCommand : IAsyncRequest<SuggestionViewModel>
    {
        private readonly IUser user;

        public SuggestionCommand(IUser user, string query, bool isDir = false, bool excludeMember = false, int? custId = null)
        {
            this.user = user;
            Query = query;
            DirectDownline = isDir;
            ExcludeMember = excludeMember;
            CustId = custId ?? user.Id;
        }

        public int UserId => user.Id;

        public int CustId { get; }

        public bool IsInternal => user.IsInternal;

        public bool IsSubAcc => false;

        public string Query { get; set; }

        public bool ExcludeMember { get; }

        public bool DirectDownline { get; }

        public bool IsAccInfo => false;
    }
}