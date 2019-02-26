namespace Alpha.CN88.Site.ViewModels.NickName
{
    using global::CN88.Auth.Models;
    using Helpers;

    public class CreateNickNameViewModel
    {
        public CreateNickNameViewModel()
        {
        }

        public CreateNickNameViewModel(SecurityResult result)
        {
            Result = result;
        }

        public SecurityResult Result { get; }

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}