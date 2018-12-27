describe("AgentList", () => {
    const template = {
        normalMode: `<body>
                        <table id="tbl-member">
                            <tbody>
                                <tr>
                                    <td>
                                        <div class="view-downline"></div>
                                        <div class="update-credit"></div>
                                        <div class="update-status"></div>
                                        <div class="icon-edit"></div>
                                        <div class="icon-add"></div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="btn-submit-member"></div>
                        <input id="HasUpdatePermission" value="1" />
                        <input id="HasAddNewPermission" value="1" />
                        <input id="directDownline" value="3" />
                        <input id="txt-viewed-master" value="" />
                        <input id="txt-username-agent" value="" />
                        <div class="shadow" id="shadow"></div>
                        <div class="output" id="output"></div>
                    </body>`
    };

    let agentList: AgentList;
    const statusComponent = new SingleSelectionDropdownComponent($);
    const pager = new PagerComponent($);
    const topWindow = { popupManager: { open: function (url, title) { } } };
    let renderList: Function;


    beforeEach(() => {
        agentList = new AgentList($, statusComponent, numeral, pager);
        spyOn(agentList, "viewAgents").and.callThrough();
        spyOn($, "blockUI");
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            spyOn(AgentList.prototype, 'init').and.callThrough();

            // act
            agentList.init({}, {});
        });
        it("_Calls super.init", () => {
            // assert
            expect(AgentList.prototype.init).toHaveBeenCalled();
        });
    });
    describe("_renderAgentList", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);

            // act
            agentList.init({
                NoInformationIsAvailable: "NoInformationIsAvailable", Disabled: "Disabled"
            }, { getCustomerColor: () => "StatusColor", getStatusName: () => "StatusName" });
        });
        it("_Empty result_render empty row", () => {
            // arrange
            agentList.$tblReport = $("#tbl-member");

            // act
            agentList.renderAgentList({ Customers: [] });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr><td colspan="7" align="center" class="no-info">NoInformationIsAvailable</td></tr>');
        });
        it("_Has data_render rows", () => {
            // arrange
            agentList.$tblReport = $("#tbl-member");

            // act
            agentList.renderAgentList({
                Customers: [
                    { CustId: 1, Username: "Username", Name: "Name", Credit: 1000, CreationDateStr: "CreationDateStr", Status: 1 }
                ]
            });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr class="row-customer StatusColor" cust-id="1"><td class="text-center">1</td><td class="view-downline" cust-id="1">Username</td><td>Name</td><td class="text-right update-credit" cust-id="1" username="Username">1,000.00</td><td class="text-center">CreationDateStr</td><td class="text-center update-status" cust-id="1" username="Username" current-status="1">StatusName</td><td class="text-center"><span class="icon-edit" cust-id="1" username="Username"></span><span class="icon-add" cust-id="1" username="Username"></span></td></tr>');
        });
    });
    describe("_viewAgentsOfMaster ", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            agentList.init({}, {});

            // act
            agentList.viewAgentsOfMaster(1, "CustName");
        });
        it("_Calls super.init", () => {
            // assert
            expect(agentList.viewAgents).toHaveBeenCalled();
        });
    });
    describe("_viewAgents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            agentList.init({}, {});

            // act
            agentList.viewAgents();
        });

        it("_call blockUI", () => {
            // assert
            expect(agentList.$.blockUI).toHaveBeenCalled();
        });
    });


});