namespace Alpha.CN88.Site.Requests.CustomerList
{
    using Alpha.CN88.Models;
    using MediatR;

    public class CreditCommand : BaseQuery, IRequest<Result>
    {
        private readonly IUser viewer;

        public CreditCommand(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public decimal CreditAmount { get; set; }
    }
}