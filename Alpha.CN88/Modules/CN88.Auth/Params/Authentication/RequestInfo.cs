namespace CN88.Auth.Params.Authentication
{
    public class RequestInfo
    {
        public RequestInfo(
            string loginIP,
            string loginDomain,
            string browserName,
            string platform,
            string countryCode)
        {
            LoginIP = loginIP;
            LoginDomain = loginDomain;
            BrowserName = browserName;
            Platform = platform;
            CountryCode = countryCode;
        }

        public string LoginIP { get; }

        public string LoginDomain { get; }

        public string BrowserName { get; }

        public string Platform { get; }

        public string CountryCode { get; }
    }
}
