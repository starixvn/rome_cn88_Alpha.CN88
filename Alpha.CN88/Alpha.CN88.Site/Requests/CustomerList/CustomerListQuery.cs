namespace Alpha.CN88.Site.Requests.CustomerList
{
    using System.Web.Mvc;
    using CN88.Models;
    using MediatR;
    using ViewModels.CustomerList;

    public class CustomerListQuery : BaseQuery, IRequest<CustomerListViewModel>
    {
        public CustomerListQuery(IUser viewer) : base(viewer)
        {
        }

        [AllowHtml]
        public string DownlineUsername { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public int SearchRoleId { get; set; }

        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}