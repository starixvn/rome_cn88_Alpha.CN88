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
            AdminName = viewer.AdminName;
            ShowCN88SaveButton = viewer.CustomerStatus == Constants.CustomerStatus.Active && viewer.AccountManagementEditView;
            ShowProblemAccountIcon = viewer.CustomerStatus == Constants.CustomerStatus.Active && viewer.AccountManagementViewOnly;
        }

        public int OnlineUserId { get; }

        public bool IsInternal { get; }

        public bool ClearClientData { get; }

        public string NickName { get; }

        public string AdminName { get; }

        public bool ShowCN88SaveButton { get; }

        public bool ShowProblemAccountIcon { get; }
    }
}