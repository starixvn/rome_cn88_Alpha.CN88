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

        public string DownlineUsername { get; set; }

        public string Username { get; set; }

        public byte SearchRoleId { get; set; }

        public byte Status { get; set; }

        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 1;
    }
}