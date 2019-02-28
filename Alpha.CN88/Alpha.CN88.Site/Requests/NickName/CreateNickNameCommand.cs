namespace Alpha.CN88.Site.Requests.NickName
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class CreateNickNameCommand : BaseQuery, IRequest<SecurityResult>
    {
        public CreateNickNameCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string NickName { get; set; }

        public string LanguageKey { get; }

        public IUser GetViewer()
            => viewer;

        public override bool IsValid()
            => base.IsValid()
            && !viewer.IsInternal;
    }
}