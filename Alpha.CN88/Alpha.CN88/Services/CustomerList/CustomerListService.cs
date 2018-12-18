namespace Alpha.CN88.Services.CustomerList
{
    using Criteria.CustomerList;
    using DataAccesses;
    using Models.CustomerList;
    using Params.CustomerList;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);
    }

    public class CustomerListServiceImpl : CustomerListService
    {
        private readonly CustomerListDataAccess customerListDataAccess;

        public CustomerListServiceImpl(CustomerListDataAccess customerListDataAccess)
        {
            this.customerListDataAccess = customerListDataAccess;
        }

        public CustomerListModel GetCustomerList(CustomerListParam param)
        {
            var criteria = new CustomerListCriteria(param);

            return customerListDataAccess.GetCustomerList(criteria);
        }
    }
}
