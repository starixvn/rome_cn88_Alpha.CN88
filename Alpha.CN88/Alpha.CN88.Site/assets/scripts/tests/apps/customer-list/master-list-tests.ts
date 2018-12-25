describe("MasterList", () => {
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
                    </body>`
    };

    let masterList: MasterList;
    const statusComponent = new SingleSelectionDropdownComponent($);
    const pager = new PagerComponent($);
    const topWindow = { popupManager: { open: function (url, title) { } } };
    let renderList: Function;


    beforeEach(() => {
        masterList = new MasterList($, statusComponent, numeral, pager);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            spyOn(MasterList.prototype, 'init').and.callThrough();

            // act
            masterList.init({}, {});
        });
        it("_Calls super.init", () => {
            // assert
            expect(MasterList.prototype.init).toHaveBeenCalled();
        });
    });
    describe("_renderMasterList", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);

            // act
            masterList.init({
                NoInformationIsAvailable: "NoInformationIsAvailable", Disabled: "Disabled"
            }, { getCustomerColor: () => "StatusColor", getStatusName: () => "StatusName" });
        });
        it("_Empty result_render empty row", () => {
            // arrange
            masterList.$tblReport = $("#tbl-member");

            // act
            masterList.renderMasterList({ Customers: [] });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr><td colspan="7" align="center" class="no-info">NoInformationIsAvailable</td></tr>');
        });
        it("_Has data_render rows", () => {
            // arrange
            masterList.$tblReport = $("#tbl-member");

            // act
            masterList.renderMasterList({
                Customers: [
                    { CustId: 1, Username: "Username", Name: "Name", Credit: 1000, CreationDateStr: "CreationDateStr", Status: 1 }
                ]
            });

            // assert
            expect($("#tbl-member > tbody").html()).toEqual('<tr class="row-customer StatusColor" cust-id="1"><td class="text-center">1</td><td class="view-downline" cust-id="1">Username</td><td>Name</td><td class="text-right update-credit" cust-id="1" username="Username">1,000</td><td class="text-center">CreationDateStr</td><td class="text-center update-status" cust-id="1" username="Username" current-status="1">StatusName</td><td class="text-center"><span class="icon-edit" cust-id="1" username="Username"></span><span class="icon-add" cust-id="1" username="Username"></span></td></tr>');
        });
    });
});