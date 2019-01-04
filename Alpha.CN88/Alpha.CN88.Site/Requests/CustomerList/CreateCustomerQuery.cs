namespace Alpha.CN88.Site.Requests.CustomerList
{
    using CN88.Models;

    public class CreateCustomerQuery : BaseQuery
    {
        public CreateCustomerQuery(IUser viewer) : base(viewer)
        {
            HasAddNewPermission = viewer.HasAddNewPermission;
        }

        public bool HasAddNewPermission { get; }
    }
}