/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />

class CustomerList {
	$: JQueryStatic;
	topWindow: any;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    directDownline: string;
    $navTabs: JQuery;
    dcs_AddCustomerLink: string;
    dcs_EditCustomerLink: string;

    viewResources = {
        NoInformationIsAvailable: '',
        Open: '',
        Closed: '',
        Suspended: '',
        Page: '',
        Of: '',
        Pager_DisplayItems: ''
    };

    masterList: MasterList;
    agentList: AgentList;
    memberList: MemberList;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
		pager: PagerComponent,
		topWindow: any) {
		this.$ = $;
		this.topWindow = topWindow;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;

        this.masterList = new MasterList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.agentList = new AgentList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.memberList = new MemberList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
    }

    init = () => {
        this.cacheDOMElements();

        this.masterList.init(this.viewResources, this);
        this.agentList.init(this.viewResources, this);
        this.memberList.init(this.viewResources, this);

        this.$navTabs.find("li[level='" + this.directDownline + "']").addClass("active");
        this.$(".tab-content[level='" + this.directDownline + "']").removeClass("display-none");

        var self = this;
        this.$(".lnk-changetab").click(function () {
            var $this = self.$(this);
            self.$navTabs.find("> li").removeClass("active");
            $this.addClass("active");

            self.changeTab($this.attr("level"));
        });

        this.$(".form-inline").bind("keypress", function (e) {
            if (e.keyCode == 13) {
                return false;
            }
        });

        switch (parseInt(this.directDownline)) {
            case siteInfrastructure.roleId.master:
                this.masterList.viewMasters();
                break;
            case siteInfrastructure.roleId.agent:
                this.agentList.viewAgents();
                break;
            case siteInfrastructure.roleId.member:
                this.memberList.viewMembers();
                break;
        }
    };

    cacheDOMElements = () => {
        this.directDownline = this.$("#directDownline").val();
        this.$navTabs = this.$("#nav-tabs");

        this.viewResources.NoInformationIsAvailable = this.$("#NoInformationIsAvailable").val();
        this.viewResources.Open = this.$("#Open").val();
        this.viewResources.Closed = this.$("#Closed").val();
        this.viewResources.Suspended = this.$("#Suspended").val();

        this.viewResources.Page = this.$("#Page").val();
        this.viewResources.Of = this.$("#of").val();
        this.viewResources.Pager_DisplayItems = this.$("#Pager_DisplayItems").val();

        this.dcs_AddCustomerLink = this.$("#DCS_AddCustomerLink").val();
        this.dcs_EditCustomerLink = this.$("#DCS_EditCustomerLink").val();
    };
    changeTab = function (level: string) {
        this.$(".tab-content").addClass("display-none");
        this.$(".tab-content[level='" + level + "']").removeClass("display-none");

        switch (parseInt(level)) {
            case siteInfrastructure.roleId.agent:
                this.agentList.initAutoComplete();
                break;
            case siteInfrastructure.roleId.member:
                this.memberList.initAutoComplete();
                break;
        }
    };

    getCustomerColor = (customer: any) => {
        if (customer.Status == siteInfrastructure.customerStatus.open) {
            return "row-open";
        }

        if (customer.Status == siteInfrastructure.customerStatus.suspended) {
            return "row-suspended";
        }

        return "row-closed reason-" + customer.CloseReason;
    };

	updateCredit = (custId: any, username: any) => {
		let url = this.$("#creditPopupUrl").val(),
			caption = this.$("#UpdateCredit").val();

		this.topWindow.ageWnd.open(url, `${caption} - ${username}`, undefined, undefined, 700, 400);       
    };
	updateStatus = (custId: any, username: any) => {
		let url = this.$("#statusPopupUrl").val(),
			caption = this.$("#UpdateStatus").val();

		this.topWindow.ageWnd.open(url, `${caption} - ${username}`, undefined, undefined, 700, 400);       
    };
    updateAccount = (custId: any, username: any) => {
        window.location.href = this.dcs_EditCustomerLink + "?" + this.$.param({
            custId: custId,
            username: username
        });
    };
    addAgentForMaster = (custId: any, username: any) => {
        window.location.href = this.dcs_AddCustomerLink + "?" + this.$.param({
            custId: custId,
            username: username
        });
    };
    addMemberForAgent = (custId: any, username: any) => {
        window.location.href = this.dcs_AddCustomerLink + "?" + this.$.param({
            custId: custId,
            username: username
        });
    };
    getStatusName = (status: number) => {
        if (status == siteInfrastructure.customerStatus.open) {
            return this.viewResources.Open;
        }

        if (status == siteInfrastructure.customerStatus.closed) {
            return this.viewResources.Closed;
        }

        return this.viewResources.Suspended;
    };
    viewMaster = (custId: any, username: any) => {
        this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.agent + "']").click();
        this.agentList.viewAgentsOfMaster(custId, username);
    };
    viewAgent = (custId: any, username: any) => {
        this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.member + "']").click();
        this.memberList.viewMembersOfAgent(custId, username);
    };
};

$(() => {
    const customerList: any = new CustomerList(
        $,
        new SingleSelectionDropdownComponent($),
        numeral,
		new PagerComponent($),
		window.top);
    customerList.init();
});
