namespace CN88.Web.Models
{
    using Sunplus.Agent.Model.Entities;

    public class AccountPermissionParam : BelongingParam
    {
        private const byte Active = 4;
        private const string CustomerStatusSessionKey = "CUSTOMERSTATUS";

        public AccountPermissionParam(string currentURL, bool isHttpPost, IUserSession userSession = null)
            : base(currentURL, userSession)
        {
            userSession = userSession ?? GetUserSession();
            ActorStatus = userSession.GetProperty<byte>(CustomerStatusSessionKey);
            ActorSubPageAccess = userSession.SubPageAccess;

            IsHttpPost = isHttpPost;
        }

        public byte ActorStatus { get; }

        public bool ActorStatusIsNotActive => ActorStatus != Active;

        public bool ActorIsMainAccount => ActorSubAccountId == 0;

        public PageAccess ActorSubPageAccess { get; }

        public bool IsHttpPost { get; }
    }
}
