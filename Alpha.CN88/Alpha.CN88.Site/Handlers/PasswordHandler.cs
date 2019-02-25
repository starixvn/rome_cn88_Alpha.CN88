namespace Alpha.CN88.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.Password;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Utilities;
    using ViewModels.Password;
    using Web.Helpers;

    public class PasswordHandler :
        IRequestHandler<ChangePasswordCommand, SecurityResult>,
        IAsyncRequestHandler<ResetMainAccountPasswordCommand, SecurityResult>,
        IAsyncRequestHandler<ResetSubAccountPasswordCommand, SecurityResult>,
        IRequestHandler<ForceChangePasswordQuery, ForceChangePasswordViewModel>
    {
        private const bool IsDisableCasino = true;
        private const string HashKeySessionKey = "Alpha.CN88.Site.Handlers.PasswordHandler.PasswordHash";

        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;
        private readonly PasswordService passwordService;

        public PasswordHandler(
            Encryptor encryptor,
            SessionManager sessionManager,
            PasswordService passwordService)
        {
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
            this.passwordService = passwordService;
        }

        public SecurityResult Handle(ChangePasswordCommand message)
            => message.IsSubAccount
            ? passwordService.ChangeSubAccountPassword(ToChangeSubAccountPasswordParam(message))
            : passwordService.ChangeMainAccountPassword(ToChangeMainAccountPasswordParam(message));

        public async Task<SecurityResult> Handle(ResetMainAccountPasswordCommand message)
            => await passwordService.ResetMainAccountPasswordAsync(new ResetMainAccountPasswordParam(
                message.CustId,
                message.SubAccountId,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                IsDisableCasino,
                message.ActorLevel.IsAgentLevel(),
                message.IP,
                message.ViewerId,
                message.ViewerName,
                message.LanguageKey,
                message.InvalidPasswords));

        public async Task<SecurityResult> Handle(ResetSubAccountPasswordCommand message)
            => await passwordService.ResetSubAccountPasswordAsync(new ResetSubAccountPasswordParam(
                message.CustId,
                message.SubAccountId,
                message.NewPassword,
                sessionManager.Get<string>(HashKeySessionKey),
                message.IP,
                message.ViewerId,
                message.ViewerName,
                message.LanguageKey,
                message.InvalidPasswords));

        public ForceChangePasswordViewModel Handle(ForceChangePasswordQuery message)
            => new ForceChangePasswordViewModel(message.ViewerId, message.SubAccountId, message.ActorLevel.Value, GetHasKey());

        private string GetHasKey()
        {
            var hashKey = encryptor.RandomString(new Random().Next(4, 8));
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
                message.ViewerName,
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
                message.ViewerName,
                message.LanguageKey,
                message.InvalidPasswords);
    }
}