namespace Alpha.CN88.Site.Handlers
{
    using MediatR;
    using Requests.CustomerList;
    using ViewModels.CustomerList;

    public class CustomerListHandler :
         IRequestHandler<IndexQuery, IndexViewModel>
    {
        public IndexViewModel Handle(IndexQuery message)
        {
            return new IndexViewModel();
        }
    }
}