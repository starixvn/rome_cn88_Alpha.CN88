namespace CN88.Auth.Services
{
    using AppResources;
    using Criteria.Password;
    using Fanex.Data.Repository;
    using Models;
    using Params.Password;

    public interface PasswordService
    {
        SecurityResult ChangeMainAccountPassword(ChangeMainAccountPasswordParam param);

        SecurityResult ChangeSubAccountPassword(ChangeSubAccountPasswordParam param);

        SecurityResult ResetDownlinePassword(ResetDownlinePasswordParam param);

        SecurityResult ResetSubAccountPassword(ResetSubAccountPasswordParam param);
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

        public SecurityResult ResetDownlinePassword(ResetDownlinePasswordParam param)
            => ResetPassword(param, param.IsResetMemberPassword);

        public SecurityResult ResetSubAccountPassword(ResetSubAccountPasswordParam param)
            => ResetPassword(param, false);

        private SecurityResult ChangePassword(PasswordParamBase param)
        {
            var result = passwordValidator.ValidateEmpty(param.CurrentPassword, param.NewPassword);

            if (result.IsFail)
            {
                return result;
            }

            var rawCurrentPassword = passwordValidator.Decrypt(param.CurrentPassword, param.HashKey);
            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            result = passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords, rawCurrentPassword);

            if (result.IsFail)
            {
                return result;
            }

            param.CurrentPassword = passwordValidator.Encode(rawCurrentPassword);
            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return Execute(param.ToCommand());
        }

        private SecurityResult ResetPassword(PasswordParamBase param, bool isResetMemberPassword)
        {
            var result = passwordValidator.ValidateEmpty(param.NewPassword);

            if (result.IsFail)
            {
                return result;
            }

            var rawNewPassword = passwordValidator.Decrypt(param.NewPassword, param.HashKey);
            result = isResetMemberPassword
                ? passwordValidator.IsMatchMemberPasswordRule(rawNewPassword, param.InvalidPasswords)
                : passwordValidator.IsMatchSMAPasswordRule(rawNewPassword, param.InvalidPasswords);

            if (result.IsFail)
            {
                return result;
            }

            param.NewPassword = passwordValidator.Encode(rawNewPassword);

            return Execute(param.ToCommand());
        }

        private SecurityResult Execute(PasswordNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.PasswordChangedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
