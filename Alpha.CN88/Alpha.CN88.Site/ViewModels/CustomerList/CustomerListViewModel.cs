namespace Alpha.CN88.Site.ViewModels.CustomerList
{
    using System.Collections.Generic;
    using System.Web;
    using CN88.Models;
    using CN88.Models.CustomerList;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(CustomerListModel model)
        {
            TotalCustomers = model.TotalCustomers;
            Customers = model.Customers;

            foreach (var c in Customers)
            {
                c.Name = HttpUtility.HtmlEncode(c.Name);
                c.Group = HttpUtility.HtmlEncode(c.Group);
            }
        }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}