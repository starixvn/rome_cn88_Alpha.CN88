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
        Task<Result> ChangeMainAccountPasswordAsync(ChangeMainAccountPasswordParam param);

        Task<Result> ChangeSubAccountPasswordAsync(ChangeSubAccountPasswordParam param);

        Task<Result> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param);

        Task<Result> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param);
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

        public async Task<Result> ChangeMainAccountPasswordAsync(ChangeMainAccountPasswordParam param)
            => await ChangeSubAccountPasswordAsync(param);

        public async Task<Result> ChangeSubAccountPasswordAsync(ChangeSubAccountPasswordParam param)
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

        public async Task<Result> ResetMainAccountPasswordAsync(ResetMainAccountPasswordParam param)
            => await ResetPasswordAsync(param, param.IsResetMemberPassword);

        public async Task<Result> ResetSubAccountPasswordAsync(ResetSubAccountPasswordParam param)
            => await ResetPasswordAsync(param, false);

        private async Task<Result> ResetPasswordAsync(ResetSubAccountPasswordParam param, bool isResetMemberPassword)
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

        private async Task<Result> ExecuteAsync(PasswordNonQueryCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return Result.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.UpdatedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
