namespace Alpha.CN88.Site.Handlers
{
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.Password;
    using global::CN88.Auth.Services;
    using global::CN88.Web.Helpers;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.Password;

    public class PasswordHandler :
        IRequestHandler<ChangePasswordCommand, SecurityResult>,
        IRequestHandler<ResetDownlinePasswordCommand, SecurityResult>,
        IRequestHandler<ResetSubAccountPasswordCommand, SecurityResult>,
        IRequestHandler<ChangePasswordQuery, ChangePasswordViewModel>,
        IRequestHandler<ResetPasswordQuery, ResetPasswordViewModel>
    {
        private const bool IsDisableCasino = true;
        private const string HashKeySessionKey = "Alpha.CN88.Site.Handlers.PasswordHandler.PasswordHash";

        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;
        private readonly IUserSession userSession;
        private readonly PasswordService passwordService;

        public PasswordHandler(
            Encryptor encryptor,
            SessionManager sessionManager,
            IUserSession userSession,
            PasswordService passwordService)
        {
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
            this.userSession = userSession;
            this.passwordService = passwordService;
        }

        public SecurityResult Handle(ChangePasswordCommand message)
        {
            var result = message.IsSubAccount
               ? passwordService.ChangeSubAccountPassword(ToChangeSubAccountPasswordParam(message))
               : passwordService.ChangeMainAccountPassword(ToChangeMainAccountPasswordParam(message));

            if (result.IsSuccess)
            {
                // Update FirstLogin or PasswordExpired.
                if (!userSession.IsLogged)
                {
                    userSession.IsLogged = true;
                }

                if (userSession.IsPasswordExpired)
                {
                    userSession.IsPasswordExpired = false;
                }
            }

            return result;
        }

        public SecurityResult Handle(ResetDownlinePasswordCommand message)
            => passwordService.ResetDownlinePassword(ToResetDownlinePasswordParam(message));

        public SecurityResult Handle(ResetSubAccountPasswordCommand message)
            => passwordService.ResetSubAccountPassword(ToResetSubAccountPasswordParam(message));

        public ChangePasswordViewModel Handle(ChangePasswordQuery message)
            => new ChangePasswordViewModel(GetHashKey(), message.Result);

        public ResetPasswordViewModel Handle(ResetPasswordQuery message)
            => new ResetPasswordViewModel(
                message.CustId,
                message.CustName,
                message.CustLevel,
                GetHashKey(),
                message.Result);

        private string GetHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        private ChangeMainAccountPasswordParam ToChangeMainAccountPasswordParam(ChangePasswordCommand message)
            => new ChangeMainAccountPasswordParam(
                message.CustId,
                message.CurrentPassword,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                IsDisableCasino,
                message.IP,
                message.ViewerId,
                message.LanguageKey,
                message.InvalidPasswords);

        private ChangeSubAccountPasswordParam ToChangeSubAccountPasswordParam(ChangePasswordCommand message)
            => new ChangeSubAccountPasswordParam(
                message.CustId,
                message.SubAccountId,
                message.CurrentPassword,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                message.IP,
                message.ViewerId,
                message.LanguageKey,
                message.InvalidPasswords);

        private ResetDownlinePasswordParam ToResetDownlinePasswordParam(ResetDownlinePasswordCommand message)
            => new ResetDownlinePasswordParam(
                message.CustId,
                message.SubAccountId,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                IsDisableCasino,
                message.IP,
                message.ViewerId,
                message.LanguageKey,
                message.InvalidPasswords);

        private ResetSubAccountPasswordParam ToResetSubAccountPasswordParam(ResetSubAccountPasswordCommand message)
            => new ResetSubAccountPasswordParam(
                message.ViewerId,
                message.CustId,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                message.IP,
                message.ViewerId,
                message.LanguageKey,
                message.InvalidPasswords);
    }
}
