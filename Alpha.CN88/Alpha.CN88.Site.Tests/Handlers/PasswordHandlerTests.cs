using System.Linq;
using Alpha.CN88.Models;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests.Password;
using Alpha.CN88.Site.ViewModels.Password;
using CN88.Auth.Models;
using CN88.Auth.Params.Password;
using CN88.Auth.Services;
using CN88.Web.Helpers;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class PasswordHandlerTests
    {
        private const string HashKeySessionKey = "Alpha.CN88.Site.Handlers.PasswordHandler.PasswordHash";

        private IUser viewer;
        private Encryptor encryptor;
        private SessionManager sessionManager;
        private IUserSession userSession;
        private PasswordService passwordService;
        private PasswordHandler handler;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            viewer = Substitute.For<IUser>();
            encryptor = Substitute.For<Encryptor>();
            encryptor.RandomString(Arg.Any<int>()).Returns("HashKey");
            sessionManager = Substitute.For<SessionManager>();
            sessionManager.Get<string>(HashKeySessionKey).Returns("HashKey");
            userSession = Substitute.For<IUserSession>();
            passwordService = Substitute.For<PasswordService>();
            handler = new PasswordHandler(encryptor, sessionManager, userSession, passwordService);
            compareLogic = new CompareLogic();
        }

        [Test]
        public void HandleChangePasswordCommand_IsMainAccount_CallsChangeMainAccountPassword()
        {
            // arrange
            userSession.IsPasswordExpired.Returns(true);
            var message = new ChangePasswordCommand(viewer);
            passwordService.ChangeMainAccountPassword(Arg.Any<ChangeMainAccountPasswordParam>()).Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            passwordService.Received().ChangeMainAccountPassword(
                Arg.Is<ChangeMainAccountPasswordParam>(
                    p => p.CustId == message.CustId
                        && p.CurrentPassword == message.CurrentPassword
                        && p.NewPassword == message.NewPassword
                        && p.HashKey.Equals("HashKey")
                        && p.IsCasinoDisabled
                        && p.IP == message.IP
                        && p.ActorId == message.ViewerId
                        && p.LanguageKey == message.LanguageKey
                        && p.InvalidPasswords.SequenceEqual(message.InvalidPasswords.ToList())));
        }

        [Test]
        public void HandleChangePasswordCommand_IsSubAccount_CallsChangeMainAccountPassword()
        {
            // arrange
            userSession.IsPasswordExpired.Returns(true);
            viewer.IsSubAcc.Returns(true);
            var message = new ChangePasswordCommand(viewer);
            passwordService.ChangeSubAccountPassword(Arg.Any<ChangeSubAccountPasswordParam>()).Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            passwordService.Received().ChangeSubAccountPassword(
                Arg.Is<ChangeSubAccountPasswordParam>(
                    p => p.CustId == message.CustId
                        && p.SubAccountId == message.SubAccountId
                        && p.CurrentPassword == message.CurrentPassword
                        && p.NewPassword == message.NewPassword
                        && p.HashKey.Equals("HashKey")
                        && p.IP == message.IP
                        && p.ActorId == message.ViewerId
                        && p.LanguageKey == message.LanguageKey
                        && p.InvalidPasswords.SequenceEqual(message.InvalidPasswords.ToList())));
        }

        [Test]
        public void HandleResetDownlinePasswordCommand_Always_CallsResetDownlinePassword()
        {
            var message = new ResetDownlinePasswordCommand(viewer);

            handler.Handle(message);

            passwordService.ResetDownlinePassword(
                Arg.Is<ResetDownlinePasswordParam>(
                    p => p.CustId == message.CustId
                        && p.NewPassword == message.NewPassword
                        && p.HashKey.Equals("HashKey")
                        && p.IsCasinoDisabled
                        && p.IP == message.IP
                        && p.ActorId == message.ViewerId
                        && p.LanguageKey == message.LanguageKey
                        && p.InvalidPasswords.SequenceEqual(message.InvalidPasswords.ToList())));
        }

        [Test]
        public void HandleResetSubAccountPasswordCommand_Always_CallsResetSubAccountPassword()
        {
            var message = new ResetSubAccountPasswordCommand(viewer);

            handler.Handle(message);

            passwordService.ResetSubAccountPassword(
                Arg.Is<ResetSubAccountPasswordParam>(
                    p => p.CustId == message.ViewerId
                        && p.SubAccountId == message.CustId
                        && p.NewPassword == message.NewPassword
                        && p.HashKey.Equals("HashKey")
                        && p.IP == message.IP
                        && p.ActorId == message.ViewerId
                        && p.LanguageKey == message.LanguageKey
                        && p.InvalidPasswords.SequenceEqual(message.InvalidPasswords.ToList())));
        }

        [Test]
        public void HandleChangePasswordQuery_Always_ReturnsChangePasswordViewModel()
        {
            var message = new ChangePasswordQuery(viewer);
            var expectedResult = new ChangePasswordViewModel("HashKey", message.Result);

            var actualResult = handler.Handle(message);

            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }

        [Test]
        public void HandleResetPasswordQuery_Always_ReturnsResetPasswordViewModel()
        {
            var message = new ResetPasswordQuery(viewer);
            var expectedResult = new ResetPasswordViewModel(message.CustId, message.CustName, message.CustLevel, "HashKey", message.Result);

            var actualResult = handler.Handle(message);

            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }
    }
}