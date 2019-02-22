namespace Alpha.CN88.Site.Handlers
{
    using System;
    using System.Threading.Tasks;
    using global::CN88.Auth.AppResources;
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.Password;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.Password;
    using Sunplus.Agent.Utilities;
    using ViewModels.Password;
    using Web.Helpers;

    public class PasswordHandler :
        IAsyncRequestHandler<ChangePasswordCommand, Result>,
        IAsyncRequestHandler<ResetMainAccountPasswordCommand, Result>,
        IAsyncRequestHandler<ResetSubAccountPasswordCommand, Result>,
        IRequestHandler<ForceChangePasswordQuery, ForceChangePasswordViewModel>
    {
        private const bool IsDisableCasino = true;
        private const string HashKeySessionKey = "Alpha.CN88.Site.PasswordHandler.PasswordHash";

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

        public async Task<Result> Handle(ChangePasswordCommand message)
            => message.IsSubAccount
            ? await ChangeSubAccountPasswordAsync(message)
            : await ChangeMainAccountPasswordAsync(message);

        public async Task<Result> Handle(ResetMainAccountPasswordCommand message)
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

        public async Task<Result> Handle(ResetSubAccountPasswordCommand message)
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
            => new ForceChangePasswordViewModel
            {
                HashKey = GetHasKey(),
                IsDefaultAvatar = true,//profile.UseDefaultAvatar,
                Username = message.ViewerName,
                AlternateUsername = BuildAlternateUserName(message),
                UsernameShower = BuildUsernameShower(message),
                EmbedImage = string.Empty//GenerateImage(profile)
            };

        private string GetHasKey()
        {
            var hashKey = encryptor.RandomString(new Random().Next(4, 8));
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        private string BuildAlternateUserName(ForceChangePasswordQuery message)
            => message.IsSubAccount
            ? $"({AuthResource.SubAcc.ToUpper()} {message.SubUserName})"
            : string.IsNullOrEmpty(message.NickName) ? string.Empty : $"({message.NickName})";

        private string BuildUsernameShower(ForceChangePasswordQuery message)
            => message.IsSubAccount
            ? message.SubUserName
            : string.IsNullOrEmpty(message.NickName) ? message.ViewerName : message.NickName;

        private async Task<Result> ChangeMainAccountPasswordAsync(ChangePasswordCommand message)
            => await passwordService.ChangeMainAccountPasswordAsync(new ChangeMainAccountPasswordParam(
                    message.CustId,
                    message.CurrentPassword,
                    message.NewPassword,
                    sessionManager.Get<string>(HashKeySessionKey),
                    IsDisableCasino,
                    message.IP,
                    message.ViewerId,
                    message.ViewerName,
                    message.LanguageKey,
                    message.InvalidPasswords));

        private async Task<Result> ChangeSubAccountPasswordAsync(ChangePasswordCommand message)
            => await passwordService.ChangeSubAccountPasswordAsync(new ChangeSubAccountPasswordParam(
                    message.CustId,
                    message.SubAccountId,
                    message.CurrentPassword,
                    message.NewPassword,
                    sessionManager.Get<string>(HashKeySessionKey),
                    message.IP,
                    message.ViewerId,
                    message.ViewerName,
                    message.LanguageKey,
                    message.InvalidPasswords));
    }
}