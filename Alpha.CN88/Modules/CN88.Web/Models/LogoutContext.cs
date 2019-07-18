namespace CN88.Web.Models
{
    using Sunplus.Agent.Model.Entities;

    public class LogoutContext
    {
        public LogoutContext(IUserSession userSession, string reason = " Kicked out: Session Changed")
        {
            OnlineUserId = userSession.OnUserId;
            UserName = userSession.IsSubAcc ? userSession.SubUserName : userSession.UserName;
            RoleId = userSession.RoleId;
            ServerId = userSession.ServerId;
            NickName = userSession.NickName;
            Reason = reason;
        }

        public int OnlineUserId { get; }

        public string UserName { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string NickName { get; }

        public string Reason { get; }
    }
}
