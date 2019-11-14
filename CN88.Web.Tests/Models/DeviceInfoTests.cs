namespace CN88.Web.Tests.Models
{
    using CN88.Web.Models;
    using NUnit.Framework;

    [TestFixture]
    public class DeviceInfoTests
    {
        [Test]
        public void Properties_Always_AssignsCorrectValue()
        {
            // Arrange
            var param = new DeviceInfo
            {
                IP = "::1",
                DeviceName = "Desktop",
                DeviceVersion = "1.0.0.6",
                BrowserName = "Chrome",
                BrowserVersion = "Gecko 1.6.9"
            };

            // Act & Assert
            Assert.IsTrue("::1" == param.IP
                && "Desktop" == param.DeviceName
                && "1.0.0.6" == param.DeviceVersion
                && "Chrome" == param.BrowserName
                && "Gecko 1.6.9" == param.BrowserVersion);
        }
    }
}
