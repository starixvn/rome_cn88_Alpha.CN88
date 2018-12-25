namespace Alpha.CN88.Site.Requests.CustomerList
{
    using CN88.Models;
    using MediatR;
    using ViewModels.CustomerList;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            HasUpdatePermission = viewer.HasUpdatePermission;
            HasAddNewPermission = viewer.HasAddNewPermission;
        }

        public bool IsInternal { get; }

        public bool HasUpdatePermission { get; }

        public bool HasAddNewPermission { get; }
    }
}