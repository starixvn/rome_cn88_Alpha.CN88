namespace Alpha.CN88.Site.Requests.Authentication
{
    using CN88.Models;
    using MediatR;

    public class LogoutQuery : BaseQuery, IAsyncRequest<bool>
    {
        public LogoutQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}