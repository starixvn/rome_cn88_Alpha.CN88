namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;
    using MediatR;
    using ViewModels;

    public class SuggestionCommand : IAsyncRequest<SuggestionViewModel>
    {
        private readonly IUser user;

        public SuggestionCommand(IUser user, string query)
        {
            this.user = user;
            Query = query;
        }

        public int CustId => user.Id;

        public bool IsInternal => user.IsInternal;

        public bool IsSubAcc => user.IsSubAcc;

        public string Query { get; set; }

        public bool ExcludeMember => false;

        public bool DirectDownline => false;

        public bool IsAccInfo => false;
    }
}