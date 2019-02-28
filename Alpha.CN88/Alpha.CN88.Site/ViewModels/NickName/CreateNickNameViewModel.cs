namespace Alpha.CN88.Site.ViewModels.NickName
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using Helpers;
    using Requests;

    public class CreateNickNameViewModel : BaseQuery
    {
        public CreateNickNameViewModel(IUser viewer)
            : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public bool IsInternal { get; }

        public SecurityResult Result { get; set; }

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}