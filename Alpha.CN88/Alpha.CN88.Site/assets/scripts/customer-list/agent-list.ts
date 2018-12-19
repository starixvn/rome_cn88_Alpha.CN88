/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../site-infrastructure.ts" />
declare var initAutoComplete: any;

class AgentList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $dropdownStatusAgent: JQuery;
    $viewedMaster: JQuery;
    $usernameAgent: JQuery;
    $tblAgent: JQuery;
    $errAgent: JQuery;
    getCustomerListUrl: string;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    paramAgent = {
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
            new SingleSelectionDropdownOption("dropdown-status-agent", "UserSelectedStatusIdAgent"));
        this.$dropdownStatusAgent = this.$("#dropdown-status-agent");

        this.$("#btn-submit-agent").click(this.viewAgents);

        if (this.directDownline >= siteInfrastructure.roleId.master) {
            this.initAutoComplete();
        }

        this.$tblAgent.stickyTableHeaders();
    };

    cacheDOMElements = () => {
        this.$viewedMaster = this.$("#txt-viewed-master");
        this.$usernameAgent = this.$("#txt-username-agent");
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblAgent = this.$("#tbl-agent");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");

        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    viewAgents = () => {
        this.$.blockUI();
        this.paramAgent = {
            DownlineUsername: this.$("#txt-viewed-master").val(),
            Username: this.$usernameAgent.val(),
            SearchRoleId: siteInfrastructure.roleId.agent,
            Status: this.$dropdownStatusAgent.find("option:selected").val(),
            PageSize: this.paramAgent.PageSize,
            PageIndex: this.paramAgent.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            this.paramAgent.DownlineUsername == "") {
            this.$errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errAgent.addClass("display-none");
        this.$.ajax({
            method: "GET",
            data: this.paramAgent,
            url: this.getCustomerListUrl,
            cache: false,
            success: this.renderAgentList
        });
    };
    renderAgentList = (rs: any) => {
        this.$tblAgent.removeClass("display-none");

        var html = "";
        var customerIndex = this.paramAgent.PageSize * (this.paramAgent.PageIndex - 1) + 1;

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

        this.$tblAgent.find("> tbody").empty().html(html);

        var self = this;
        this.$tblAgent.find(".view-downline").click(function () {
            var $this = self.$(this);

            self.indexManager.viewAgent($this.attr("cust-id"), $this.html());
        });
        this.$tblAgent.find(".update-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblAgent.find(".update-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblAgent.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"));
        });
        this.$tblAgent.find(".icon-add").click(function () {
            var $this = self.$(this);

            self.indexManager.addMemberForAgent($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblAgent.find("> tfoot").empty();
        this.$("#PageIndexAgent").remove();
        this.$("#PageSizeAgent").remove();
        if (rs.TotalCustomers > siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='7'><div id='report-pager-agent' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.paramAgent.PageIndex + "'" +
                "data-pagesize='" + this.paramAgent.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";

            this.$tblAgent.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingAgentList, "report-pager-agent", "form-agent", "PageIndexAgent", "PageSizeAgent"));
        }

        this.$.unblockUI();
    };
    pagingAgentList = () => {
        this.paramAgent.PageIndex = this.$("#PageIndexAgent").val();
        this.paramAgent.PageSize = this.$("#PageSizeAgent").val();
        this.viewAgents();
    };
    viewAgentsOfMaster = (custId: any, username: any) => {
        this.$viewedMaster.val(username);
        this.$usernameAgent.val("");

        this.paramAgent.PageIndex = 1;
        this.paramAgent.PageSize = 100;

        this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);

        this.viewAgents();
    };
    initAutoComplete = () => {
        var queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: 0, issub: 0, ismember: 0 });
        initAutoComplete(this.$viewedMaster[0], queryUsernameURL);
        this.$output.css("top", this.$viewedMaster.offset().top + 30);
        this.$output.css("left", this.$viewedMaster.offset().left);
    };
}

