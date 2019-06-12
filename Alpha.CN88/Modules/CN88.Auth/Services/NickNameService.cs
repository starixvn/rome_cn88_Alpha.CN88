namespace CN88.Auth.Services
{
    using System.Text.RegularExpressions;
    using AppResources;
    using Criteria.NickName;
    using Fanex.Data.Repository;
    using Models;
    using Params.NickName;

    public interface NickNameService
    {
        SecurityResult CreateNickName(CreateNickNameParam param);
    }

    public class NickNameServiceImpl : NickNameService
    {
        private static readonly Regex NickNameRule = new Regex(@"^[a-zA-Z0-9]{5,20}$");
        private const int InvalidNickNameErrorCode = 100;

        private readonly IDynamicRepository dynamicRepository;

        public NickNameServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public SecurityResult CreateNickName(CreateNickNameParam param)
        {
            if (!IsValidNickName(param.NickName))
            {
                return SecurityResult.CreateErrorResult(AuthResource.NickNameRule1, InvalidNickNameErrorCode);
            }

            return param.IsSubAccount
                ? CreateSubAccountNickName(param)
                : CreateAndSetNickNameLoginOnly(param);
        }

        private static bool IsValidNickName(string nickName)
            => NickNameRule.IsMatch(nickName);

        private SecurityResult CreateSubAccountNickName(CreateNickNameParam param)
            => Execute(new CreateSubAccountNickNameCommand(param));

        private SecurityResult CreateAndSetNickNameLoginOnly(CreateNickNameParam param)
        {
            var result = Execute(new CreateNickNameCommand(param));

            return result.IsFail
                ? result
                : Execute(new UpdateNickNameLoginOnlyCommand(param, true));
        }

        private SecurityResult Execute(NickNameNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.ConfiguredNickNameSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
