namespace Alpha.CN88.Site.Requests.Home
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Home;

    public class HomeQuery : BaseQuery, IRequest<HomeViewModel>
    {
        public HomeQuery(IUser viewer)
            : base(viewer)
        {
            ViewMemberInfo = viewer.ViewMemberInfo;
        }

        public bool ViewMemberInfo { get; }
    }
}