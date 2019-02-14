namespace Alpha.CN88.Site.Helpers
{
    using System.Collections;
    using System.Web;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Core.Common.IPs;
    using UAParser;

    public static class DeviceHelper
    {
        public static Hashtable GetRequestInfo(string browserSize)
        {
            var ip = AgentIPFormat.GetClientIP();
            var country = "Unknown";
            var browserName = string.Empty;
            var platform = string.Empty;

            try
            {
                var infoIp = IpUtils.LocationFromIP(ip);
                country = infoIp.CountryLong == "Reserved" ? null : infoIp.CountryLong;
            }
            catch
            {
            }

            try
            {
                var userAgentParser = Parser.GetDefault();
                var clientInfo = userAgentParser.Parse(GetRequest().UserAgent ?? string.Empty);

                browserName = GetBrowserName(clientInfo.UserAgent, browserSize);
                platform = GetPlatform(clientInfo);
            }
            catch
            {
            }

            return new Hashtable
            {
                ["loginip"] = ip,
                ["countrycode"] = country,
                ["logindomain"] = GetRequest().ServerVariables["SERVER_NAME"],
                ["browsername"] = browserName,
                ["platform"] = platform
            }; ;
        }

        private static HttpRequest GetRequest()
            => HttpContext.Current.Request;

        private static string GetBrowserName(UserAgent userAgent, string browserSize)
        {
            var browser = $"{userAgent.Family} {userAgent.Major}.{userAgent.Minor}.{userAgent.Patch ?? "0"}";

            return string.IsNullOrWhiteSpace(browserSize)
                ? browser
                : browser + $", {browserSize}";
        }

        private static string GetPlatform(ClientInfo clientInfo)
        {
            var deviceInfo = clientInfo.Device;
            var device = $"{deviceInfo.Family}, IsCrawler: {deviceInfo.IsSpider}";

            var os = clientInfo.OS;
            var majorVersion = os.Major ?? "0";
            var minorVersion = os.Minor ?? "0";
            var osVersion = os.Patch ?? "0";
            var operateSystem = $"{os.Family} {majorVersion}.{minorVersion}.{osVersion}";

            return $"{operateSystem}, Device: {device}";
        }
    }
}