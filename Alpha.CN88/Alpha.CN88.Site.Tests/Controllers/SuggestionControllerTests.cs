using Alpha.CN88.Site.Controllers;
using Alpha.CN88.Site.Requests;
using Alpha.CN88.Site.Tests.TestHelpers;
using Alpha.CN88.Site.Tests.TestModels;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using NSubstitute;
using NUnit.Framework;

namespace Alpha.CN88.Site.Tests.Controllers
{
    public class SuggestionControllerTests : ControllerTestTemplate<SuggestionController>
    {
        UserTest user;
        IMediator mediator;

        protected override SuggestionController EstablishContext()
        {
            user = new UserTest();
            mediator = Substitute.For<IMediator>();

            return new SuggestionController(user, mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void Index_Always_CallsMeditorSendSuggestionCommand()
        {
            // Arrange
            var query = "query";
            var isDir = false;
            var isMember = false;
            int? custId = 1;
            var command = new SuggestionCommand(user, query, isDir, isMember, custId);

            // Act
            Sut.Index(query, isDir, isMember, custId).ConfigureAwait(false);

            // Assert
            mediator.Received().SendAsync(Arg.Is<SuggestionCommand>(c => new CompareLogic().Compare(c, command).AreEqual));
        }
    }
}
