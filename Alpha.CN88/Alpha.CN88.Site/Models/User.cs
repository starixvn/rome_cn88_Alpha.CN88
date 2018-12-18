namespace Alpha.CN88.Site.Models
{
    using CN88.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public bool IsInternal => userSession.IsInternal;
        
        public int SubAccountId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;
    }
}