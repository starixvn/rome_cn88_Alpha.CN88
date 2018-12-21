namespace Alpha.CN88.Site.Requests.CustomerList
{
    using Alpha.CN88.Models;
    using MediatR;

    public class UpdateSuspendedCommand : BaseQuery, IRequest<Result>
    {
        public UpdateSuspendedCommand(IUser viewer) : base(viewer)
        {
        }

        public bool Suspended { get; set; }
    }
}