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

        SecurityResult UpdateNickNameLoginOnly(UpdateNickNameLoginOnlyParam param);
    }

    public class NickNameServiceImpl : NickNameService
    {
        private const int NickNameRule1ErrCode = 100;
        private static readonly Regex NickNameRule = new Regex(@"^[a-zA-Z0-9]{5,15}$");

        private readonly IDynamicRepository dynamicRepository;

        public NickNameServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public SecurityResult CreateNickName(CreateNickNameParam param)
            => IsValidNickName(param.NickName)
            ? Execute(param.ToCommand())
            : SecurityResult.CreateErrorResult(AuthResource.NickNameRule1, NickNameRule1ErrCode);

        public SecurityResult UpdateNickNameLoginOnly(UpdateNickNameLoginOnlyParam param)
            => Execute(param.ToCommand());

        private static bool IsValidNickName(string nickName)
            => NickNameRule.IsMatch(nickName);

        private SecurityResult Execute(NickNameNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.ConfiguredNickNameSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
