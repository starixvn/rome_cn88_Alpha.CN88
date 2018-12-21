namespace Alpha.CN88.Site.Requests.CustomerList
{
    using Alpha.CN88.Models;
    using MediatR;

    public class UpdateClosedCommand : BaseQuery, IRequest<Result>
    {
        public UpdateClosedCommand(IUser viewer) : base(viewer)
        {
        }

        public bool Closed { get; set; }
    }
}