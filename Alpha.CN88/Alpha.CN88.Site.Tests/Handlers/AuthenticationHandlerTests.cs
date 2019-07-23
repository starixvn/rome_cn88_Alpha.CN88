namespace Alpha.CN88.Site.Tests.Handlers
{
    using Alpha.CN88.Site.Handlers;
    using Alpha.CN88.Site.Requests.Authentication;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Auth.Params.Authentication;
    using global::CN88.Auth.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationHandlerTests
    {
        AuthenticationService authenticationService { get; set; }
        AuthenticationHandler Sut;

        [SetUp]
        public void SetUp()
        {
            authenticationService = Substitute.For<AuthenticationService>();

            Sut = new AuthenticationHandler(authenticationService);
        }

        [Test]
        public void Handle_LoginQuery_CallServiceLogin()
        {
            // Arrange
            var loginQuery = new LoginQuery();
            loginQuery.Params = "query_param";
            loginQuery.IBCKeyPath = "path";

            // Act
            Sut.Handle(loginQuery);

            // Assert
            authenticationService.Received(1).Login("query_param", "path");
        }

        [Test]
        public void Handle_AdminLoginQuery_CallServiceAdminLogin()
        {
            // Arrange
            var loginQuery = new AdminLoginQuery();
            loginQuery.Params = "query_param";
            loginQuery.IBCKeyPath = "path";

            // Act
            Sut.Handle(loginQuery);

            // Assert
            authenticationService.Received(1).AdminLogin("query_param", "path");
        }

        [Test]
        public void Handle_LogoutQuery_CallServiceLogout()
        {
            // Arrange
            var user = new UserTest();
            var loginQuery = new LogoutQuery(user);

            // Act
            Sut.Handle(loginQuery);

            // Assert
            authenticationService.Received(1).Logout(Arg.Any<LogoutParam>());
        }
    }
}
