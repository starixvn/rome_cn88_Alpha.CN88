namespace Alpha.CN88.Site.Tests.TestHelpers
{
    using KellermanSoftware.CompareNetObjects;
    using NUnit.Framework;

    [TestFixture]
    public abstract class TestTemplate<TContext>
    {
        protected virtual TContext Sut { get; private set; }
        protected CompareLogic compareLogic { get; private set; }

        [SetUp]
        public virtual void MainSetup()
        {
            compareLogic = new CompareLogic();
            Sut = EstablishContext();
        }

        [TearDown]
        public void Teardown()
        {
            TestCleanup();
        }

        protected abstract TContext EstablishContext();

        protected abstract void TestCleanup();
    }
}
