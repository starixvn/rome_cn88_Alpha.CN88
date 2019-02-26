namespace Alpha.CN88.Site.ViewModels.Password
{
    using global::CN88.Auth.Models;
    using Helpers;

    public class ChangePasswordViewModel : PasswordViewModel
    {
        public ChangePasswordViewModel(
            string hashKey,
            SecurityResult result) : base(hashKey, result)
        {
        }

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}