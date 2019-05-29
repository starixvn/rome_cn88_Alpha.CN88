namespace Alpha.CN88.Site.Requests.Default
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Default;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
            OnlineUserId = viewer.OnlineUserId;
            IsInternal = viewer.IsInternal;
            ClearClientData = viewer.ClearClientData;
            NickName = viewer.NickName;
        }

        public int OnlineUserId { get; }

        public bool IsInternal { get; }

        public bool ClearClientData { get; }

        public string NickName { get; }
    }
}