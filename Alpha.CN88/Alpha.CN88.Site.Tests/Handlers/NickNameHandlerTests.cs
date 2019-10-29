using Alpha.CN88.Models;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests.NickName;
using CN88.Auth.Models;
using CN88.Auth.Params.NickName;
using CN88.Auth.Services;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class NickNameHandlerTests
    {
        private IUser user;
        private NickNameService nickNameService;
        private IUserSession userSession;
        private NickNameHandler handler;

        [SetUp]
        public void SetUp()
        {
            user = Substitute.For<IUser>();
            nickNameService = Substitute.For<NickNameService>();
            userSession = Substitute.For<IUserSession>();
            handler = new NickNameHandler(nickNameService, userSession);
        }

        [Test]
        public void HandleCreateNickNameCommand_Always_CallsCreateNickName()
        {
            user.SubAccountId.Returns(1);
            var message = new CreateNickNameCommand(user);
            nickNameService.CreateNickName(Arg.Any<CreateNickNameParam>()).Returns(new SecurityResult());

            handler.Handle(message);

            nickNameService.Received().CreateNickName(Arg.Is<CreateNickNameParam>(
                p => p.CustId == message.ViewerId
                && p.SubAccountId == message.SubAccountId
                && p.IP == message.IP
                && p.LanguageKey == message.LanguageKey));
        }
    }
}