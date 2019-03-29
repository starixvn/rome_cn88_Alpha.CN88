namespace Alpha.CN88.Site.Requests.Default
{
    using CN88.Models;
    using MediatR;

    public class TotalOnlineMember : BaseQuery, IRequest<long>
    {
        public TotalOnlineMember(IUser viewer)
            : base(viewer)
        {
        }
    }
}