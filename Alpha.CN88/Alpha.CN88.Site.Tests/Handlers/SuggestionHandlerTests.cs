using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.CN88.Models;
using Alpha.CN88.Params;
using Alpha.CN88.Services;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests;
using Alpha.CN88.Site.ViewModels;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class SuggestionHandlerTests
    {
        private IUser user;
        private SuggestionService suggestionService;
        private SuggestionHandler handler;

        [SetUp]
        public void SetUp()
        {
            user = Substitute.For<IUser>();
            suggestionService = Substitute.For<SuggestionService>();
            handler = new SuggestionHandler(suggestionService);
        }

        [Test]
        public void HandleSuggestionCommand_Always_ReturnsSuggestionViewModel()
        {
            var message = new SuggestionCommand(user, "query");
            IEnumerable<SuggestionModel> suggestionModel = new List<SuggestionModel>
            {
                new SuggestionModel("username", "firstname", "lastname", "username2")
            };
            suggestionService.SearchUser(Arg.Any<SuggestionParam>()).Returns(Task.FromResult(suggestionModel));
            var expectedResult = new SuggestionViewModel(message.Query, suggestionModel.Select(i => new[] { i.Username, i.Firstname, i.Lastname, i.Username2 }));

            var actualResult = handler.Handle(message).GetAwaiter().GetResult();

            Assert.IsTrue(new CompareLogic().Compare(expectedResult, actualResult).AreEqual);
        }
    }
}