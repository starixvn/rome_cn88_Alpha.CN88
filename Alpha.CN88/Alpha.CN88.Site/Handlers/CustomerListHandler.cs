namespace Alpha.CN88.Site.Handlers
{
    using MediatR;
    using Params.CustomerList;
    using Requests.CustomerList;
    using Services.CustomerList;
    using ViewModels.CustomerList;

    public class CustomerListHandler :
         IRequestHandler<IndexQuery, IndexViewModel>,
         IRequestHandler<CustomerListQuery, CustomerListViewModel>
    {
        private readonly CustomerListService service;

        public CustomerListHandler(CustomerListService service)
        {
            this.service = service;
        }

        public IndexViewModel Handle(IndexQuery message)
            => new IndexViewModel(message);

        public CustomerListViewModel Handle(CustomerListQuery message)
        {
            var param = new CustomerListParam(
                message.ViewerId,
                message.DownlineUsername,
                message.SearchRoleId,
                message.Username,
                message.Status,
                message.PageSize,
                message.PageIndex);
            var model = service.GetCustomerList(param);

            return new CustomerListViewModel(model);
        }
    }
}