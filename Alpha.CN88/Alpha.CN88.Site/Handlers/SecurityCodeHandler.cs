namespace Alpha.CN88.Site.Handlers
{
    using Fanex.Cryptography.Aes;
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
        IRequestHandler<ChangeSecurityCodeCommand, SecurityResult>,
        IRequestHandler<ResetSecurityCodeCommand, SecurityResult>,
        IRequestHandler<ResetSubAccountSecurityCodeCommand, SecurityResult>,
        IRequestHandler<VerifySecurityCodeCommand, SecurityResult>,
        IRequestHandler<ChangeSecurityCodeQuery, ChangeSecurityCodeViewModel>,
        IRequestHandler<ResetSecurityCodeQuery, ResetSecurityCodeViewModel>,
        IRequestHandler<VerifySecurityCodeQuery, VerifySecurityCodeViewModel>
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

        public SecurityResult Handle(ChangeSecurityCodeCommand message)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);
            var param = new ChangeSecurityCodeParam(
                    message.ViewerId,
                    message.SubAccountId,
                    message.OldSecurityCode,
                    message.SecurityCode,
                    sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                    message.ViewerId,
                    message.IP,
                    message.LanguageKey);

            var result = message.SubAccountId == 0
                ? securityCodeService.ChangeMainAccountSecurityCode(param)
                : securityCodeService.ChangeSubAccountSecurityCode(param);

            if (result.IsSuccess)
            {
                ResetToken();

                if (message.IsForceChangeSecurityCode)
                {
                    userSession.SecurityCode = message.SecurityCode;
                }
            }

            return result;
        }

        public SecurityResult Handle(ResetSecurityCodeCommand message)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);
            var param = new ChangeSecurityCodeParam(
                message.IsForceChangeFirstLogin ? message.ViewerId : message.CustId,
                message.SubAccountId,
                message.SecurityCode,
                message.ViewerId,
                message.IP,
                message.LanguageKey);
            SecurityResult result;

            if (message.IsForceChangeFirstLogin)
            {
                result = message.SubAccountId == 0
                    ? securityCodeService.ChangeMainAccountSecurityCode(param)
                    : securityCodeService.ChangeSubAccountSecurityCode(param);

                if (result.IsSuccess)
                {
                    ResetToken();
                    userSession.SecurityCode = message.SecurityCode;
                }
            }
            else
            {
                // Reset downline security code.
                result = securityCodeService.ChangeMainAccountSecurityCode(param);
            }

            return result;
        }

        public SecurityResult Handle(ResetSubAccountSecurityCodeCommand message)
        {
            message.SecurityCode = Decrypt(message.SecurityCode);

            return securityCodeService.ResetSubAccountSecurityCode(
                new ResetSecurityCodeParam(
                    message.ViewerId,
                    message.CustId,
                    message.SecurityCode,
                    message.ViewerId,
                    message.IP,
                    message.LanguageKey));
        }

        public SecurityResult Handle(VerifySecurityCodeCommand message)
        {
            var result = securityCodeService.VerifySecurityCode(new VerifySecurityCodeParam(
                message.ViewerId,
                message.SubAccountId,
                message.SecurityCode,
                sessionManager.Get<string>(SecurityCodeTokenSessionKey),
                message.IP,
                message.LanguageKey));

            if (result.IsSuccess)
            {
                ResetToken();
            }

            return result;
        }

        public ChangeSecurityCodeViewModel Handle(ChangeSecurityCodeQuery message)
        {
            var securityCodeToken = securityCodeService.GenerateSecurityToken();
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new ChangeSecurityCodeViewModel(
                message.ViewerName,
                securityCodeToken,
                GenerateSecuritySalt(message.ViewerId, message.SubAccountId),
                HashKey,
                message.Result);
        }

        public ResetSecurityCodeViewModel Handle(ResetSecurityCodeQuery message)
            => new ResetSecurityCodeViewModel(message.CustId, message.CustName, HashKey, message.Result);

        public VerifySecurityCodeViewModel Handle(VerifySecurityCodeQuery message)
        {
            var securityCodeToken = securityCodeService.GenerateSecurityToken();
            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new VerifySecurityCodeViewModel(
                securityCodeToken,
                GenerateSecuritySalt(message.ViewerId, message.SubAccountId));
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
                    hashKey = encryptor.RandomString(4);
                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return hashKey;
            }
        }

        private string Decrypt(string encryptedText)
            => string.IsNullOrWhiteSpace(encryptedText)
            ? string.Empty
            : AesExtension.Decrypt(encryptedText, HashKey);

        private void ResetToken()
        {
            sessionManager.Add(SecurityCodeTokenSessionKey, string.Empty);
            userSession.IsForcedSecurityCode = false;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;
        }
    }
}