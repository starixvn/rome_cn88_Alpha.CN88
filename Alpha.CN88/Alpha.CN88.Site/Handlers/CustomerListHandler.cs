namespace Alpha.CN88.Site.Handlers
{
    using Alpha.CN88.Models;
    using MediatR;
    using Params.CustomerList;
    using Requests.CustomerList;
    using Services.CustomerList;
    using ViewModels.CustomerList;

    public class CustomerListHandler :
         IRequestHandler<IndexQuery, IndexViewModel>,
         IRequestHandler<CustomerListQuery, CustomerListViewModel>,
         IRequestHandler<CreditPopupQuery, CreditPopupViewModel>,
         IRequestHandler<StatusPopupQuery, StatusPopupViewModel>,
         IRequestHandler<CreditCommand, Result>,
         IRequestHandler<UpdateClosedCommand, Result>,
         IRequestHandler<UpdateSuspendedCommand, Result>
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

        public CreditPopupViewModel Handle(CreditPopupQuery message)
           => new CreditPopupViewModel(message);

        public StatusPopupViewModel Handle(StatusPopupQuery message)
         => new StatusPopupViewModel(message);

        public Result Handle(CreditCommand message)
           => service.UpdateCredit(
               new UpdateCreditParam(
                   message.CustId,
                   message.SearchAccountRoleId,
                   message.CreditAmount,
                   message.ViewerId,
                   message.SubAccountId,
                   message.Language,
                   message.IP));

        public Result Handle(UpdateClosedCommand command)
            => service.UpdateClosed(new UpdateClosedParam(
               command.SearchAccountId,
               command.ViewerId,
               command.SubAccountId,
               command.Language,
               command.IP,
               command.ViewerName,
               command.Closed));

        public Result Handle(UpdateSuspendedCommand command)
           => service.UpdateSusspended(new UpdateSuspendedParam(
              command.SearchAccountId,
              command.ViewerId,
              command.SubAccountId,
              command.Language,
              command.IP,
              command.ViewerName,
              command.Suspended));
    }
}