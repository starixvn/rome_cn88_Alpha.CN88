namespace Alpha.CN88.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.CustomerList;
    using Fanex.Common;
    using Fanex.Data;
    using Models;
    using Models.CustomerList;

    public interface CustomerListDataAccess
    {
        CustomerListModel GetCustomerList(CustomerListCriteria criteria);
    }

    public class CustomerListDataAccessImpl : CustomerListDataAccess
    {
        public CustomerListModel GetCustomerList(CustomerListCriteria criteria)
        {
            // return CustomerListStub.MasterList(criteria);

            if (!criteria.IsValid())
            {
                return new CustomerListModel();
            }

            var totalCustomers = 0;
            var customers = new List<CustomerModel>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerModel(
                                custId: Converter.ToInt(reader["CustId"]),
                                username: Converter.ToString2(reader["Username"]),
                                name: Converter.ToString2(reader["FirstName"]),
                                credit: Converter.ToDecimal(reader["Credit"]),
                                creationDate: Converter.ToDateTime(reader["CreatedDate"]),
                                status: Converter.ToInt(reader["Status"]),
                                closeReason: Converter.ToInt(reader["ClosedReason"])));
                        }

                        reader.Close();
                    }
                }

                totalCustomers = Converter.ToInt(objectDb.GetOuputParams()["totalrows"]);
            }

            return new CustomerListModel(totalCustomers, customers);
        }
    }
}
