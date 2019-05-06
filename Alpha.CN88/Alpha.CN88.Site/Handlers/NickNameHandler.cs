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
            var nickName = message.NickName == null ? string.Empty : message.NickName.Trim();
            var param = new CreateNickNameParam(message.ViewerId, message.SubAccountId, nickName, message.IP, message.LanguageKey);

            var result = nickNameService.CreateNickName(param);

            if (result.IsSuccess)
            {
                userSession.NickName = nickName;

                if (param.IsSubAccount)
                {
                    userSession.IsNicknameLoginOnly = true;
                }
            }

            return result;
        }
    }
}