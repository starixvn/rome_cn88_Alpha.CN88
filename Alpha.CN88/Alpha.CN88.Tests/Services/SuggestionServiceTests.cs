namespace Alpha.CN88.Tests.Services
{
    using Alpha.CN88.Models;
    using Alpha.CN88.Params;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SuggestionServiceTests
    {
        IDynamicRepository repository;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IDynamicRepository>();
        }

        [Test]
        public void SearchUser_Always_CallRepositoryFetchAsync()
        {
            // Arrange
            var service = new SuggestionServiceImpl(repository);
            var param = new SuggestionParam(1, 1, true, "a", true, true, true, true);

            // Act
            service.SearchUser(param);

            // Assert
            repository.Received(1).FetchAsync<SuggestionModel>(Arg.Any<ICriteria>());
        }
    }
}
