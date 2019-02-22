namespace Alpha.CN88.Site.Handlers
{
    using System;
    using global::CN88.Auth.AppResources;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.SecurityCode;
    using Sunplus.Agent.Utilities;
    using ViewModels.SecurityCode;
    using Web.Helpers;

    public class SecurityCodeHandler :
        IRequestHandler<ForceChangeSecurityCodeQuery, ForceChangeSecurityCodeViewModel>
    {
        private const string HashKeySessionKey = "ph";
        private const string SecurityCodeTokenSessionKey = "SECURITYCODETOKEN";

        //private readonly IUser user;
        private readonly SessionManager sessionManager;
        private readonly Encryptor encryptor;
        private readonly SecurityCodeService securityCodeService;

        public SecurityCodeHandler(
            //IUser user,
            SessionManager sessionManager,
            Encryptor encryptor,
            SecurityCodeService securityCodeService)
        {
            //this.user = user;
            this.sessionManager = sessionManager;
            this.encryptor = encryptor;
            this.securityCodeService = securityCodeService;
        }

        public ForceChangeSecurityCodeViewModel Handle(ForceChangeSecurityCodeQuery message)
        {
            //var profile = customerProfileService.GetProfilePicture(userSession.CustId);
            var securityCodeToken = securityCodeService.GenerateSecurityToken();

            sessionManager.Add(SecurityCodeTokenSessionKey, securityCodeToken);

            return new ForceChangeSecurityCodeViewModel
            {
                SecurityCodeToken = securityCodeToken,
                SecuritySalt = GenerateSecuritySalt(message.ViewerId, message.SubAccountId),
                HashKey = HashKey,
                CustRoleId = message.ViewerLevelId,
                IsDefaultAvatar = true,//profile.UseDefaultAvatar,
                Username = message.ViewerName,
                AlternateUsername = BuildAlternateUsername(message),
                UsernameShower = BuildUserNameShower(message),
                EmbedImage = string.Empty
            };
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

        private string BuildAlternateUsername(ForceChangeSecurityCodeQuery message)
            => message.IsSubAccount
            ? $"({AuthResource.SubAcc.ToUpper()} {message.SubUserName})"
            : string.IsNullOrWhiteSpace(message.NickName) ? string.Empty : $"({message.NickName})";

        private string BuildUserNameShower(ForceChangeSecurityCodeQuery message)
            => message.IsSubAccount
            ? message.SubUserName
            : string.IsNullOrWhiteSpace(message.NickName) ? message.ViewerName : message.NickName;
    }
}