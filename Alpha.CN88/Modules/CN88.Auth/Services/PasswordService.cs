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
        Task<SecurityResult> ChangeMainAccountPasswordAsync(ChangeMainAccountPasswordParam param);

        Task<SecurityResult> ChangeSubAccountPasswordAsync(ChangeSubAccountPasswordParam param);

        Task<SecurityResult> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param);

        Task<SecurityResult> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param);
    }

    public class PasswordServiceImpl : PasswordService
    {
        private const int AgentSystem = 2;

        private readonly IDynamicRepository dynamicRepository;
        private readonly PasswordValidator passwordValidator;

        public PasswordServiceImpl(
            IDynamicRepository dynamicRepository,
            PasswordValidator passwordValidator)
        {
            this.dynamicRepository = dynamicRepository;
            this.passwordValidator = passwordValidator;
        }

        public async Task<SecurityResult> ChangeMainAccountPasswordAsync(ChangeMainAccountPasswordParam param)
            => await ChangeSubAccountPasswordAsync(param);

        public async Task<SecurityResult> ChangeSubAccountPasswordAsync(ChangeSubAccountPasswordParam param)
        {
            var rawCurrentPassword = passwordValidator.Decrypt(param.CurrentPassword, param.HashKey);
            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            var validatorResult = passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords, rawCurrentPassword);

            if (validatorResult.IsFail)
            {
                return validatorResult;
            }

            param.CurrentPassword = passwordValidator.Encode(rawCurrentPassword);
            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return await ExecuteAsync(param.ToCriteria());
        }

        public async Task<SecurityResult> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param)
            => await ResetPasswordAsync(param, param.IsResetMemberPassword);

        public async Task<SecurityResult> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param)
            => await ResetPasswordAsync(param, false);

        private async Task<SecurityResult> ResetPasswordAsync(ResetSubAccountPasswordParam param, bool isResetMemberPassword)
        {
            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            var validatorResult = isResetMemberPassword
                ? passwordValidator.IsMatchMemberPasswordRule(rawNewPassword, param.InvalidPasswords)
                : passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords);

            if (validatorResult.IsFail)
            {
                return validatorResult;
            }

            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return await ExecuteAsync(param.ToCriteria());
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
