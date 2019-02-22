namespace CN88.Auth.Models
{
    public class Result
    {
        private const int DefaultErrorCode = 1;

        public Result()
            : this(0, string.Empty)
        {
        }

        public Result(int code, string message)
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

        public static Result CreateErrorResult(string message, int? code = null)
            => new Result(code.HasValue ? code.Value : DefaultErrorCode, message);
    }
}
