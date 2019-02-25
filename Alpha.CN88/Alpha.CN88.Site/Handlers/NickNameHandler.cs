namespace Alpha.CN88.Site.Handlers
{
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Params.NickName;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.NickName;
    using Sunplus.Agent.Model.Entities;

    public class NickNameHandler : IRequestHandler<CreateNickNameCommand, SecurityResult>
    {
        private readonly NickNameService nickNameService;
        private readonly IUserSession userSession;

        public NickNameHandler(
            NickNameService nickNameService,
            IUserSession userSession)
        {
            this.nickNameService = nickNameService;
            this.userSession = userSession;
        }

        public SecurityResult Handle(CreateNickNameCommand message)
        {
            var nickName = message.NickName.Trim();

            var result = nickNameService.CreateNickName(
                new CreateNickNameParam(message.ViewerId, nickName, message.IP, message.LanguageKey));

            if (result.IsSuccess)
            {
                result = nickNameService.UpdateNickNameLoginOnly(
                    new UpdateNickNameLoginOnlyParam(message.ViewerId, true, message.IP, message.LanguageKey));

                if (result.IsSuccess)
                {
                    userSession.IsNickname = true;
                    userSession.IsNicknameLoginOnly = true;
                    userSession.NickName = nickName;
                }
            }

            return result;
        }
    }
}