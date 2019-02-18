namespace Alpha.CN88.Site.Requests.Authentication
{
    using CN88.Models;
    using MediatR;

    public class LogoutQuery : BaseQuery, IRequest<bool>
    {
        public LogoutQuery(IUser viewer)
            : base(viewer)
        {
            OnlineUserId = viewer.OnlineUserId;
            SubUserName = viewer.SubUserName;
            NickName = viewer.NickName;
            ServerId = viewer.ServerId;
        }

        public int OnlineUserId { get; }

        public string SubUserName { get; }

        public string NickName { get; }

        public int ServerId { get; }
    }
}