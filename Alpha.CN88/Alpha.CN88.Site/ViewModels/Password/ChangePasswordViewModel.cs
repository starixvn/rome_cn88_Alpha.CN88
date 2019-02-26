namespace Alpha.CN88.Site.ViewModels.Password
{
    using global::CN88.Auth.Models;
    using Helpers;

    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel(
            string hashKey,
            SecurityResult result)
        {
            HashKey = hashKey;
            Result = result;
        }

        public string HashKey { get; }

        public SecurityResult Result { get; }

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}