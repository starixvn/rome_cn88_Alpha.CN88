/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../site-infrastructure.ts" />

class MasterList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $dropdownStatusMaster: JQuery;
    $usernameMaster: JQuery;
    $tblMaster: JQuery;
    getCustomerListUrl: string;

    paramMaster = {
        DownlineUsername: null,
        Username: "",
        SearchRoleId: 0,
        Status: 0,
        PageSize: 100,
        PageIndex: 1
    };
    viewResources: any;
    indexManager = null;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
    }

    init = (viewResources: any, indexManager: any) => {
        this.viewResources = viewResources;
        this.indexManager = indexManager;

        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption("dropdown-status-master", "UserSelectedStatusIdMaster"));
        this.$dropdownStatusMaster = this.$("#dropdown-status-master");

        this.$("#btn-submit-master").click(this.viewMasters);
        this.$tblMaster.stickyTableHeaders();
    };

    cacheDOMElements = () => {
        this.$usernameMaster = this.$("#txt-username-master");
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblMaster = this.$("#tbl-master");
    };
    viewMasters = () => {
        this.$.blockUI();
        this.paramMaster = {
            DownlineUsername: null,
            Username: this.$usernameMaster.val(),
            SearchRoleId: siteInfrastructure.roleId.master,
            Status: this.$dropdownStatusMaster.find("option:selected").val(),
            PageSize: this.paramMaster.PageSize,
            PageIndex: this.paramMaster.PageIndex
        };

        this.$.ajax({
            method: "GET",
            data: this.paramMaster,
            url: this.getCustomerListUrl,
            cache: false,
            success: this.renderMasterList
        });
    };
    renderMasterList = (rs: any) => {
        this.$tblMaster.removeClass("display-none");

        var html = "";
        var customerIndex = this.paramMaster.PageSize * (this.paramMaster.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='7' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                var customer = rs.Customers[i];
                html += "<tr class='" + this.indexManager.getCustomerColor(customer) + "'>" +
                    "<td class='text-center'>" + customerIndex + "</td>" +
                    "<td class='view-downline' cust-id='" + customer.CustId + "'>" + customer.Username + "</td>" +
                    "<td>" + customer.Name + "</td>" +
                    "<td class='text-right update-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.numeral(customer.Credit).format("###,0") + "</td>" +
                    "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                    "<td class='text-center update-status' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='text-center'>" +
                    "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'></span>" +
                    "<span class='icon-add' cust-id='" + customer.CustId + "' username='" + customer.Username + "'></span>" +
                    "</td>" +
                    "</tr>";
            }
        }

        this.$tblMaster.find("> tbody").empty().html(html);

        var self = this;
        this.$tblMaster.find(".view-downline").click(function () {
            var $this = self.$(this);

            self.indexManager.viewMaster($this.attr("cust-id"), $this.html());
        });
        this.$tblMaster.find(".update-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblMaster.find(".update-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblMaster.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblMaster.find(".icon-add").click(function () {
            var $this = self.$(this);

            self.indexManager.addAgentForMaster($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblMaster.find("> tfoot").empty();
        this.$("#PageIndexMaster").remove();
        this.$("#PageSizeMaster").remove();
        if (rs.TotalCustomers > siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='7'><div id='report-pager-master' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.paramMaster.PageIndex + "'" +
                "data-pagesize='" + this.paramMaster.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";

            this.$tblMaster.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingMasterList, "report-pager-master", "form-master", "PageIndexMaster", "PageSizeMaster"));
        }

        this.$.unblockUI();
    };
    pagingMasterList = () => {
        this.paramMaster.PageIndex = this.$("#PageIndexMaster").val();
        this.paramMaster.PageSize = this.$("#PageSizeMaster").val();
        this.viewMasters();
    };
}

