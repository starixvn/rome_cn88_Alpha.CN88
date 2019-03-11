namespace CN88.Web.Services
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public interface RequestContext
    {
        long GetTransId(HttpRequestBase httpRequest);

        int GetCustId(
            HttpRequestBase httpRequest,
            string[] custIdQueryStringParamNames,
            string[] custIdJsonParamNames);
    }

    internal class RequestContextImpl : RequestContext
    {
        public long GetTransId(HttpRequestBase httpRequest)
        {
            long transId;

            if (long.TryParse(httpRequest["transid"], out transId))
            {
                return transId;
            }

            long.TryParse(httpRequest["refno"], out transId);

            return transId;
        }

        public int GetCustId(
            HttpRequestBase httpRequest,
            string[] custIdQueryStringParamNames,
            string[] custIdJsonParamNames)
        {
            var custId = 0;

            foreach (var item in custIdQueryStringParamNames)
            {
                if (int.TryParse(httpRequest[item], out custId))
                {
                    return custId;
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
            var custId = 0;
            var jsonString = GetJsonDataInRequestBody(httpRequest.InputStream);

            var pattern = string.Format("(\"{0}\":)(?<paramValue>[^,}}]+)", parameterName);
            var regexMatch = Regex.Match(jsonString, pattern, RegexOptions.IgnoreCase);

            if (regexMatch.Success)
            {
                int.TryParse(regexMatch.Groups["paramValue"].ToString().Replace("\"", string.Empty), out custId);
            }

            return custId;
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
