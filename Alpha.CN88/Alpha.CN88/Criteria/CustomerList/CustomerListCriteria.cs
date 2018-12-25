namespace Alpha.CN88.Criteria.CustomerList
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params.CustomerList;

    public class CustomerListCriteria : CriteriaBase
    {
        public CustomerListCriteria(CustomerListParam param)
        {
            ViewerId = param.ViewerId;
            DownlineUsername = param.DownlineUsername;
            SearchRoleId = param.SearchRoleId;
            Username = string.IsNullOrWhiteSpace(param.Username) ? string.Empty : param.Username;
            Status = param.Status;
            PageSize = param.PageSize;
            PageIndex = param.PageIndex;
        }

        [SpParam(SqlParamName = "CustId")]
        public int ViewerId { get; set; }

        public string DownlineUsername { get; set; }

        public int SearchRoleId { get; set; }

        [SpParam(SqlParamName = "SearchUsername")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "SearchStatusId")]
        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        [SpParam(SqlParamName = "TotalRows")]
        public int TotalCustomers { get; set; }

        public override string GetSettingKey()
            => "Age_CS_Customer_DownlineList_Sel";

        public override bool IsValid()
            => ViewerId > 0;
    }
}
