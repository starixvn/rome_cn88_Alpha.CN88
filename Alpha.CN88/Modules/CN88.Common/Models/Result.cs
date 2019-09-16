namespace CN88.Common.Models
{
    public class Result
    {
        private const int SuccessResultCode = 0;
        private const int AccessDeniedCode = 1;

        public static readonly Result Success = new Result(SuccessResultCode, string.Empty);

        public Result()
        {
        }

        public Result(int code, string msg)
        {
            ErrCode = code;
            ErrMsg = msg;
        }

        public string ErrMsg { get; set; }

        public int ErrCode { get; set; }

        public bool IsSuccess => ErrCode == 0;

        public static Result GetAccessDeniedResult(string message)
            => new Result(AccessDeniedCode, message);

        public static Result CreateSuccessResult(string message = "")
            => new Result(SuccessResultCode, message);

        public static Result CreateErrorResult(int code = 1, string message = "")
            => new Result(code, message);
    }
}