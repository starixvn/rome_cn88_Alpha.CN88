using System;
using Alpha.CN88.Constants;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests;
using Alpha.CN88.Site.ViewModels;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class LanguageHandlerTests
    {
        private LanguageHandler languageHandler;

        public LanguageHandlerTests()
        {
            languageHandler = new LanguageHandler();
        }

        [Test]
        public void HandleAllLanguagesQuery_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            AllLanguagesQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => languageHandler.Handle(message));
        }

        [Test]
        public void HandleAllLanguagesQuery_MessageIsNOTNull_ReturnsLanguageViewModel()
        {
            // Arrange
            var message = new AllLanguagesQuery("en-US");
            var expectedViewModel = new LanguageViewModel(Language.CodeNames, "en-US");

            // Act
            var actualViewModel = languageHandler.Handle(message);

            // Assert
            Assert.True(new CompareLogic().Compare(expectedViewModel, actualViewModel).AreEqual);

        }

        [Test]
        public void HandleChangeLanguageCommand_MessageIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeLanguageCommand command = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => languageHandler.Handle(command));
        }

        [Test]
        public void HandleChangeLanguageCommand_MessageIsNull_ReturnsCorrectLanguageCode()
        {
            // Arrange
            var command = new ChangeLanguageCommand(Substitute.For<IUserSession>(), "en-US");

            // Act & Assert
            Assert.AreEqual("en-US", languageHandler.Handle(command));
        }
    }
}
