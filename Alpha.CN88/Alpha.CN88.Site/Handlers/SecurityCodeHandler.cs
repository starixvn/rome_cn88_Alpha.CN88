namespace Alpha.CN88.Site.Handlers
{
    using System;
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.SecurityCode;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.SecurityCode;
    using SecurityCode.Web;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using ViewModels.SecurityCode;
    using Web.Helpers;

    public class SecurityCodeHandler :
        IRequestHandler<ChangeSecurityCodeFirstLoginCommand, SecurityResult>,
        IRequestHandler<ForceChangeSecurityCodeCommand, SecurityResult>,
        IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";

        private readonly SessionManager sessionManager;
        private readonly IUserSession userSession;
        private readonly Encryptor encryptor;
        private readonly SecurityCodeService securityCodeService;

        public SecurityCodeHandler(
            SessionManager sessionManager,
            IUserSession userSession,
            Encryptor encryptor,
            SecurityCodeService securityCodeService)
        {
            this.sessionManager = sessionManager;
            this.userSession = userSession;
            this.encryptor = encryptor;
            this.securityCodeService = securityCodeService;
        }

        public SecurityResult Handle(ChangeSecurityCodeFirstLoginCommand message)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);

            return ChangeSecurityCode(ToChangeSecurityCodeParam(message));
        }

        public SecurityResult Handle(ForceChangeSecurityCodeCommand message)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);

            return ChangeSecurityCode(ToChangeSecurityCodeParam(message));
        }

        public ForceChangeSecurityCodeViewModel Handle(ForceChangeSecurityCodeQuery message)
        {
            if (string.IsNullOrWhiteSpace(message.SecurityCode))
            {
                return new ForceChangeSecurityCodeViewModel(HashKey, message.Result);
            }

            var securityCodeToken = securityCodeService.GenerateSecurityToken();
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new ForceChangeSecurityCodeViewModel(
                message.ViewerName,
                securityCodeToken,
                GenerateSecuritySalt(message.ViewerId, message.SubAccountId),
                HashKey,
                message.Result);
        }

        private string GenerateSecuritySalt(int custId, int subAccId)
            => securityCodeService.GenerateSalt(subAccId > 0 ? subAccId : custId);

        private string HashKey
        {
            get
            {
                var hashKey = sessionManager.Get<string>(HashKeySessionKey);

                if (string.IsNullOrWhiteSpace(hashKey))
                {
                    hashKey = encryptor.RandomString(new Random().Next(4, 8));

                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return hashKey;
            }
        }

        private string Decrypt(string encryptedText)
            => string.IsNullOrWhiteSpace(encryptedText)
            ? string.Empty
            : encryptor.RC4(HashKey, encryptor.HexDecode(encryptedText));

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            userSession.IsForcedSecurityCode = false;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }

        private SecurityResult ChangeSecurityCode(ChangeSecurityCodeParam param)
        {
            var result = param.SubAccId == 0
                ? securityCodeService.ChangeMainAccountSecurityCode(param)
                : securityCodeService.ChangeSubAccountSecurityCode(param);

            if (result.IsSuccess)
            {
                userSession.SecurityCode = param.NewSecurityCode;
                ResetToken();
            }

            return result;
        }

        private ChangeSecurityCodeParam ToChangeSecurityCodeParam(ChangeSecurityCodeFirstLoginCommand message)
            => new ChangeSecurityCodeParam(
                message.ViewerId,
                message.SubAccountId,
                message.SecurityCode,
                message.ViewerId,
                message.IP,
                message.LanguageKey);

        private ChangeSecurityCodeParam ToChangeSecurityCodeParam(ForceChangeSecurityCodeCommand message)
            => new ChangeSecurityCodeParam(
                message.ViewerId,
                message.SubAccountId,
                message.OldSecurityCode,
                message.SecurityCode,
                sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                message.ViewerId,
                message.IP,
                message.LanguageKey);
    }
}