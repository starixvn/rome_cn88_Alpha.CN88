namespace CN88.Web.Models
{
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class BelongingParam
    {
        public BelongingParam(string currentURL, IUserSession userSession = null)
        {
            CurrentURL = currentURL;

            userSession = userSession ?? GetUserSession();
            ActorId = userSession.CustId;
            ActorRoleId = userSession.RoleId;
            ActorSubAccountId = userSession.SubAccId;
        }

        public string CurrentURL { get; }

        public int ActorId { get; }

        public byte ActorRoleId { get; }

        public bool IsHeadline => ActorRoleId == 0;

        public int ActorSubAccountId { get; }

        protected IUserSession GetUserSession() => new UserSessionExt();
    }
}
