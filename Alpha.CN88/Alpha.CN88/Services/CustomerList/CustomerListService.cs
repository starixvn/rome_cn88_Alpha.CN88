namespace Alpha.CN88.Services.CustomerList
{
    using Alpha.CN88.Models;
    using Criteria.CustomerList;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models.CustomerList;
    using Params.CustomerList;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);

        Result UpdateCredit(UpdateCreditParam param);

        Result UpdateClosed(UpdateClosedParam param);

        Result UpdateSusspended(UpdateSuspendedParam param);
    }

    public class CustomerListServiceImpl : CustomerListService
    {
        private readonly int failErrorCode = 100;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CustomerListDataAccess customerListDataAccess;

        public CustomerListServiceImpl(IDynamicRepository dynamicRepository, CustomerListDataAccess customerListDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.customerListDataAccess = customerListDataAccess;
        }

        public CustomerListModel GetCustomerList(CustomerListParam param)
        {
            var criteria = new CustomerListCriteria(param);

            return customerListDataAccess.GetCustomerList(criteria);
        }

        public Result UpdateCredit(UpdateCreditParam param)
        {
            var command = new UpdateCreditCriteria(
                    param.SearchAccountId,
                    param.RoleId,
                    param.CreditAmount,
                    param.ActorId,
                    param.SubAccountId,
                    param.Language,
                    param.IP);

             dynamicRepository.ExecuteAsync(command);

            return command.CreditProblem
                ? Result.CreateErrorResult(failErrorCode, command.ErrMsg)
                : Result.CreateSuccessResult();
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
