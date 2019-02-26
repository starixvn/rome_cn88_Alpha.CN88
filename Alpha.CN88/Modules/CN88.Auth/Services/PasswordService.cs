namespace CN88.Auth.Services
{
    using System.Threading.Tasks;
    using AppResources;
    using Criteria.Password;
    using Fanex.Data.Repository;
    using Models;
    using Params.Password;

    public interface PasswordService
    {
        SecurityResult ChangeMainAccountPassword(ChangeMainAccountPasswordParam param);

        SecurityResult ChangeSubAccountPassword(ChangeSubAccountPasswordParam param);

        Task<SecurityResult> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param);

        Task<SecurityResult> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param);
    }

    public class PasswordServiceImpl : PasswordService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PasswordValidator passwordValidator;

        public PasswordServiceImpl(
            IDynamicRepository dynamicRepository,
            PasswordValidator passwordValidator)
        {
            this.dynamicRepository = dynamicRepository;
            this.passwordValidator = passwordValidator;
        }

        public SecurityResult ChangeMainAccountPassword(ChangeMainAccountPasswordParam param)
            => ChangePassword(param);

        public SecurityResult ChangeSubAccountPassword(ChangeSubAccountPasswordParam param)
            => ChangePassword(param);

        public async Task<SecurityResult> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param)
            => await ResetPasswordAsync(param, param.IsResetMemberPassword);

        public async Task<SecurityResult> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param)
            => await ResetPasswordAsync(param, false);

        private SecurityResult ChangePassword(PasswordParamBase param)
        {
            if (string.IsNullOrWhiteSpace(param.CurrentPassword))
            {
                return SecurityResult.CreateErrorResult(AuthResource.RequiredInputOldPwdError);
            }

            if (string.IsNullOrWhiteSpace(param.NewPassword))
            {
                return SecurityResult.CreateErrorResult(AuthResource.RequiredPasswordError);
            }

            var rawCurrentPassword = passwordValidator.Decrypt(param.CurrentPassword, param.HashKey);
            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            var validatorResult = passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords, rawCurrentPassword);

            if (validatorResult.IsFail)
            {
                return validatorResult;
            }

            param.CurrentPassword = passwordValidator.Encode(rawCurrentPassword);
            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return Execute(param.ToCommand());
        }

        private async Task<SecurityResult> ResetPasswordAsync(ResetSubAccountPasswordParam param, bool isResetMemberPassword)
        {
            if (string.IsNullOrWhiteSpace(param.NewPassword))
            {
                return SecurityResult.CreateErrorResult(AuthResource.RequiredPasswordError);
            }

            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            var validatorResult = isResetMemberPassword
                ? passwordValidator.IsMatchMemberPasswordRule(rawNewPassword, param.InvalidPasswords)
                : passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords);

            if (validatorResult.IsFail)
            {
                return validatorResult;
            }

            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return await ExecuteAsync(param.ToCommand());
        }

        private SecurityResult Execute(PasswordNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.UpdatedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }

        private async Task<SecurityResult> ExecuteAsync(PasswordNonQueryCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.UpdatedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
