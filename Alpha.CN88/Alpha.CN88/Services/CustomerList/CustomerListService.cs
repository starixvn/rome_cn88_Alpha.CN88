namespace Alpha.CN88.Services.CustomerList
{
    using Alpha.CN88.Models;
    using Criteria.CustomerList;
    using DataAccesses;
    using Models.CustomerList;
    using Params.CustomerList;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);

        Result UpdateClosed(UpdateClosedParam param);

        Result UpdateSusspended(UpdateSuspendedParam param);
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

        public Result UpdateClosed(UpdateClosedParam param)
        {
            var result = customerListDataAccess.UpdateCustomerStatus(
                new UpdateClosedCriteria(
                    param.SearchAccountId,
                    param.ActorId,
                    param.SubAccountId,
                    param.Language,
                    param.IP,
                    param.Closed));

            return result.IsSuccess
                ? Result.CreateSuccessResult()
                : result;
        }

        public Result UpdateSusspended(UpdateSuspendedParam param)
        {
            var result = customerListDataAccess.UpdateCustomerStatus(
                new UpdateSuspendedCriteria(
                    param.SearchAccountId,
                    param.ActorId,
                    param.SubAccountId,
                    param.Language,
                    param.IP,
                    param.Suspended));

            return result.IsSuccess
                ? Result.CreateSuccessResult()
                : result;
        }
    }
}
