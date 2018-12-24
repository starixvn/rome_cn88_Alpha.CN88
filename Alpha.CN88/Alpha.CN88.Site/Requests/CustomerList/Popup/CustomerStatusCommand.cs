namespace Alpha.CN88.Site.Requests.CustomerList
{
    using Alpha.CN88.Models;
    using MediatR;

    public class CustomerStatusCommand : BaseQuery, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer) : base(viewer)
        {
        }

        public byte UpdateStatusId { get; set; }
    }
}