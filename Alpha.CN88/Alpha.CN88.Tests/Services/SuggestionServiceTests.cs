namespace Alpha.CN88.Tests.Services
{
    using Alpha.CN88.Criteria;
    using Alpha.CN88.Models;
    using Alpha.CN88.Params;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SuggestionServiceTests
    {
        private IDynamicRepository repository;
        private SuggestionServiceImpl service;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IDynamicRepository>();
            service = new SuggestionServiceImpl(repository);
        }

        [Test]
        public void SearchUser_Always_CallRepositoryFetchAsync()
        {
            // Arrange
            var param = new SuggestionParam(1, 1, true, "a", true, true, true, true);

            // Act
            service.SearchUser(param);

            // Assert
            repository.Received(1).FetchAsync<SuggestionModel>(
                Arg.Is<SuggestionCriteria>(
                    c => c.UserId == param.UserId
                        && c.CustId == param.CustId
                        && c.DirectDownline == param.DirectDownline
                        && c.ExcludeMember == param.ExcludeMember
                        && c.IsAccInfo == param.IsAccInfo
                        && c.IsInternalAgent == param.IsInternal
                        && c.IsSubAccount == param.IsSubAccount
                        && c.Username == param.Username));
        }
    }
}
