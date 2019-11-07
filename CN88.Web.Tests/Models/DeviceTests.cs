namespace CN88.Web.Tests.Models
{
    using System;
    using CN88.Web.Models;
    using NUnit.Framework;

    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public void Properties_Always_AssignsCorrectValue()
        {
            // Arrange
            var param = new Device
            {
                Id = "123xyz",
                Info = new DeviceInfo(),
                CreatedDate = new DateTime(2019, 11, 6),
                UpdatedDate = new DateTime(2019, 11, 7)
            };

            // Act & Assert
            Assert.IsTrue("123xyz" == param.Id
                && null != param.Info
                && 2019 == param.CreatedDate.Year
                && 11 == param.CreatedDate.Month
                && 6 == param.CreatedDate.Day
                && 2019 == param.UpdatedDate.Year
                && 11 == param.UpdatedDate.Month
                && 7 == param.UpdatedDate.Day);
        }
    }
}
