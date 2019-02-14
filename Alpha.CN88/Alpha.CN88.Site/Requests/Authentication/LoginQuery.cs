namespace Alpha.CN88.Site.Requests.Authentication
{
    using CN88.Models.Authentication;
    using MediatR;

    public class LoginQuery : IRequest<AuthResult>
    {
        public string Params { get; set; }

        public string IBCKeyPath { get; set; }

        public bool IsInvalid()
            => string.IsNullOrWhiteSpace(Params);
    }
}