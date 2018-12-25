describe("MemberList", () => {
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
                        <input id="txt-viewed-agent" value="" />
                        <input id="txt-username-member" value="" />
                        <div class="shadow" id="shadow"></div>
                        <div class="output" id="output"></div>
                    </body>`
    };

    let memberList: MemberList;
    const statusComponent = new SingleSelectionDropdownComponent($);
    const pager = new PagerComponent($);
    const topWindow = { popupManager: { open: function (url, title) { } } };
    let renderList: Function;

    beforeEach(() => {
        memberList = new MemberList($, statusComponent, numeral, pager);
        spyOn(memberList, "viewMembers").and.callThrough();
        spyOn($, "blockUI");
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            spyOn(MemberList.prototype, 'init').and.callThrough();

            // act
            memberList.init({}, {});
        });
        it("_Calls super.init", () => {
            // assert
            expect(MemberList.prototype.init).toHaveBeenCalled();
        });
    });
    describe("_renderMemberList", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);

            // act
            memberList.init({
                NoInformationIsAvailable: "NoInformationIsAvailable", Disabled: "Disabled"
            }, { getCustomerColor: () => "StatusColor", getStatusName: () => "StatusName" });
        });
        it("_Empty result_render empty row", () => {
            // arrange
            memberList.$tblReport = $("#tbl-member");

            // act
            memberList.renderMemberList({ Customers: [] });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr><td colspan="7" align="center" class="no-info">NoInformationIsAvailable</td></tr>');
        });
        it("_Has data_render rows", () => {
            // arrange
            memberList.$tblReport = $("#tbl-member");

            // act
            memberList.renderMemberList({
                Customers: [
                    { CustId: 1, Username: "Username", Name: "Name", Credit: 1000, CreationDateStr: "CreationDateStr", Status: 1 }
                ]
            });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr class="row-customer StatusColor" cust-id="1"><td class="text-center">1</td><td>Username</td><td>Name</td><td class="text-right update-credit" cust-id="1" username="Username">1,000</td><td class="text-center">CreationDateStr</td><td class="text-center update-status" cust-id="1" username="Username" current-status="1">StatusName</td><td class="text-center"><span class="icon-edit" cust-id="1" username="Username"></span></td></tr>');
        });
    });
    describe("_viewMembersOfAgent ", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            memberList.init({}, {});

            // act
            memberList.viewMembersOfAgent(1, "CustName");
        });
        it("_Calls super.init", () => {
            // assert
            expect(memberList.viewMembers).toHaveBeenCalled();
        });
    });
    describe("_viewMembers", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            memberList.init({}, {});

            // act
            memberList.viewMembers();
        });

        it("_call blockUI", () => {
            // assert
            expect(memberList.$.blockUI).toHaveBeenCalled();
        });
    });



});