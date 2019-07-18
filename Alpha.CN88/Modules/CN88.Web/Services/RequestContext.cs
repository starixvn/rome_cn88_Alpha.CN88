namespace CN88.Web.Services
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public interface RequestContext
    {
        int GetCustIdFromHttpRequest(
            HttpRequestBase httpRequest,
            string[] custIdQueryStringParamNames,
            string[] custIdJsonParamNames);
    }

    internal class RequestContextImpl : RequestContext
    {
        public int GetCustIdFromHttpRequest(
            HttpRequestBase httpRequest,
            string[] custIdQueryStringParamNames,
            string[] custIdJsonParamNames)
        {
            var custId = 0;
            string queryStringValue;

            foreach (var item in custIdQueryStringParamNames)
            {
                queryStringValue = httpRequest[item];

                if (!string.IsNullOrWhiteSpace(queryStringValue))
                {
                    custId = int.Parse(queryStringValue);

                    if (custId > 0)
                    {
                        return custId;
                    }
                }
            }

            return GetCustomerIdValueFromJSON(httpRequest, custIdJsonParamNames);
        }

        private static int GetCustomerIdValueFromJSON(HttpRequestBase httpRequest, string[] custIdJsonParamNames)
        {
            var custId = 0;

            foreach (var item in custIdJsonParamNames)
            {
                custId = GetCustomerId(httpRequest, item);

                if (custId > 0)
                {
                    return custId;
                }
            }

            return custId;
        }

        private static int GetCustomerId(HttpRequestBase httpRequest, string parameterName)
        {
            var jsonString = GetJsonDataInRequestBody(httpRequest.InputStream);

            var pattern = string.Format("(\"{0}\":)(?<paramValue>[^,}}]+)", parameterName);
            var regexMatch = Regex.Match(jsonString, pattern, RegexOptions.IgnoreCase);

            return regexMatch.Success
                ? int.Parse(regexMatch.Groups["paramValue"].ToString().Replace("\"", string.Empty))
                : 0;
        }

        private static string GetJsonDataInRequestBody(Stream requestStream)
        {
            if (requestStream != null)
            {
                requestStream.Seek(0, SeekOrigin.Begin);
                var bodyData = new StreamReader(requestStream).ReadToEnd();

                requestStream.Seek(0, SeekOrigin.Begin);

                if (!string.IsNullOrWhiteSpace(bodyData) && bodyData.Contains("{"))
                {
                    return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
                }
            }

            return string.Empty;
        }
    }
}
