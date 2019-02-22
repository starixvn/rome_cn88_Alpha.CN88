namespace CN88.Auth.Models
{
    public class SCResult
    {
        private const int ErrorCode = 1;

        public SCResult()
            : this(0, string.Empty)
        {
        }

        public SCResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

        public bool IsSuccessful
            => Code == 0;

        public bool IsFailed
            => !IsSuccessful;

        public bool IsOk
            => Code == 0;

        public bool IsNotOk
            => !IsOk;

        public static SCResult CreateErrorResult(string message, int? code = null)
            => new SCResult(code.HasValue ? code.Value : ErrorCode, message);
    }
}
