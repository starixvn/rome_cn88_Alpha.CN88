/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../site-infrastructure.ts" />

class MemberList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $dropdownStatusMember: JQuery;
    $viewedAgent: JQuery;
    $usernameMember: JQuery;
    $tblMember: JQuery;
    $errMember: JQuery;
    getCustomerListUrl: string;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    paramMember = {
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
            new SingleSelectionDropdownOption("dropdown-status-member", "UserSelectedStatusIdMember"));
        this.$dropdownStatusMember = this.$("#dropdown-status-member");

        this.$("#btn-submit-member").click(this.viewMembers);
        this.$tblMember.stickyTableHeaders();

        if (this.directDownline >= siteInfrastructure.roleId.agent) {
            this.initAutoComplete();
        }
    };

    cacheDOMElements = () => {
        this.$viewedAgent = this.$("#txt-viewed-agent");
        this.$usernameMember = this.$("#txt-username-member");
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblMember = this.$("#tbl-member");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errMember = this.$("#msg-err-member");

        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    viewMembers = () => {
        this.$.blockUI();
        this.paramMember = {
            DownlineUsername: this.$("#txt-viewed-agent").val(),
            Username: this.$usernameMember.val(),
            SearchRoleId: siteInfrastructure.roleId.member,
            Status: this.$dropdownStatusMember.find("option:selected").val(),
            PageSize: this.paramMember.PageSize,
            PageIndex: this.paramMember.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            this.paramMember.DownlineUsername == "") {
            this.$errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errMember.addClass("display-none");
        this.$.ajax({
            method: "GET",
            data: this.paramMember,
            url: this.getCustomerListUrl,
            cache: false,
            success: this.renderMemberList
        });
    };
    renderMemberList = (rs: any) => {
        this.$tblMember.removeClass("display-none");

        var html = "";
        var customerIndex = this.paramMember.PageSize * (this.paramMember.PageIndex - 1) + 1;

        if (rs.Customers.length == 0) {
            html = "<tr><td colspan='7' align='center' class='no-info'>" + this.viewResources.NoInformationIsAvailable + "</td></tr>";
        } else {
            for (var i = 0; i < rs.Customers.length; i++ , customerIndex++) {
                var customer = rs.Customers[i];
                html += "<tr class='row-customer " + this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                    "<td class='text-center'>" + customerIndex + "</td>" +
                    "<td>" + customer.Username + "</td>" +
                    "<td>" + customer.Name + "</td>" +
                    "<td class='text-right update-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + this.numeral(customer.Credit).format("###,0") + "</td>" +
                    "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                    "<td class='text-center update-status' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + this.indexManager.getStatusName(customer.Status) + "</td>" +
                    "<td class='text-center'>" +
                    "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'></span>" +
                    "</td>" +
                    "</tr>";
            }
        }

        this.$tblMember.find("> tbody").empty().html(html);

        var self = this;
        this.$tblMember.find(".update-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), siteInfrastructure.roleId.member);
        });
        this.$tblMember.find(".update-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"));
        });
        this.$tblMember.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblMember.find("> tfoot").empty();
        this.$("#PageIndexMember").remove();
        this.$("#PageSizeMember").remove();
        if (rs.TotalCustomers > siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='7'><div id='report-pager-member' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.paramMember.PageIndex + "'" +
                "data-pagesize='" + this.paramMember.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";

            this.$tblMember.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingMemberList, "report-pager-member", "form-member", "PageIndexMember", "PageSizeMember"));
        }

        this.$.unblockUI();
    };
    pagingMemberList = () => {
        this.paramMember.PageIndex = this.$("#PageIndexMember").val();
        this.paramMember.PageSize = this.$("#PageSizeMember").val();
        this.viewMembers();
    };
    viewMembersOfAgent = (custId: any, username: any) => {
        this.$viewedAgent.val(username);
        this.$usernameMember.val("");

        this.paramMember.PageIndex = 1;
        this.paramMember.PageSize = 100;

        this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);

        this.viewMembers();
    };
    initAutoComplete = () => {
        var queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: 0, issub: 0, ismember: 0 });
        initAutoComplete(this.$viewedAgent[0], queryUsernameURL);

        this.$output.css("top", this.$viewedAgent.offset().top + 30);
        this.$output.css("left", this.$viewedAgent.offset().left);
    };
}

