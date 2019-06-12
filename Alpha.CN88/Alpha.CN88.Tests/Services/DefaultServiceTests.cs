namespace Alpha.CN88.Tests.Services
{
    using Alpha.CN88.Criteria;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultServiceTests
    {
        IDynamicRepository repository;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IDynamicRepository>();
        }

        [Test]
        public void GetOnlineMember_Alway_CallRepositoryGetTotalMember()
        {
            // Arrange
            var service = new DefaultServiceImpl(repository);

            // Act
            service.GetOnlineMember(999);

            // Assert
            repository.Received(1).Get<int>(Arg.Any<GetTotalMemberOnlineCriteria>());
        }
    }
}
