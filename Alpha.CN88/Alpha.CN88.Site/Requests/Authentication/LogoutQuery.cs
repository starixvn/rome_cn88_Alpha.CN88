namespace Alpha.CN88.Site.Requests.Authentication
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class LogoutQuery : IRequest<bool>
    {
        public LogoutQuery(IUserSession userSession)
        {
            CurrentUserSession = userSession;
        }

        public IUserSession CurrentUserSession { get; }
    }
}