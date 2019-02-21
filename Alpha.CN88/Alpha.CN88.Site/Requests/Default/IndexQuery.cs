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
        }

        public int OnlineUserId { get; }

        public bool IsInternal { get; }
    }
}