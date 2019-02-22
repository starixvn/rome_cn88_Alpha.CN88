namespace CN88.Auth.Models
{
    public class SecurityResult
    {
        private const int DefaultErrorCode = 1;

        public SecurityResult()
            : this(0, string.Empty)
        {
        }

        public SecurityResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

        public bool IsSuccess
            => Code == 0;

        public bool IsFail
            => !IsSuccess;

        public static SecurityResult CreateErrorResult(string message, int? code = null)
            => new SecurityResult(code.HasValue ? code.Value : DefaultErrorCode, message);
    }
}
