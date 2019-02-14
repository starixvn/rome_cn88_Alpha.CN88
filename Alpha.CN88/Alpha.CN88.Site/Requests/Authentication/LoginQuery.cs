namespace Alpha.CN88.Site.Requests.Authentication
{
    using System.Web;
    using CN88.Models.Authentication;
    using MediatR;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    public class LoginQuery : IRequest<AuthResult>
    {
        private IUserSession _currentUserSession;

        public string Params { get; set; }

        public string IBCKeyPath { get; set; }

        public IUserSession CurrentUserSession
        {
            get
            {
                _currentUserSession = _currentUserSession ?? UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);

                return _currentUserSession;
            }
        }

        public bool IsInvalid()
            => string.IsNullOrWhiteSpace(Params);
    }
}