namespace Alpha.CN88.Site.ViewModels.Password
{
    using global::CN88.Auth.Models;

    public class PasswordViewModel
    {
        public PasswordViewModel(
            string hashKey,
            SecurityResult result)
        {
            HashKey = hashKey;
            Result = result;
        }

        public string HashKey { get; }

        public SecurityResult Result { get; }
    }
}