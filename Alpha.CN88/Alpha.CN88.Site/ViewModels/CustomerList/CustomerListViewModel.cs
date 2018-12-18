namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using System.Collections.Generic;
    using CN88.Models;
    using CN88.Models.CustomerList;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(CustomerListModel model)
        {
            TotalCustomers = model.TotalCustomers;
            Customers = model.Customers;
        }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}