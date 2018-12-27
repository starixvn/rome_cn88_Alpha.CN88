describe("CustomerListIndex", () => {
    let customerListIndex: CustomerList;
    const statusComponent = new SingleSelectionDropdownComponent($);
    const pager = new PagerComponent($);
    const topWindow = { popupManager: { open: function (url, title) { } } };

    let cacheElements: jasmine.Spy;

    beforeEach(() => {
        customerListIndex = new CustomerList($, statusComponent, numeral, pager, topWindow);

        cacheElements = spyOn(customerListIndex, "cacheDOMElements");
        cacheElements.and.callThrough();

        spyOn(customerListIndex.masterList, "init");
        spyOn(customerListIndex.agentList, "init");
        spyOn(customerListIndex.memberList, "init");
        spyOn(customerListIndex, "changeTab").and.callThrough();

        spyOn(customerListIndex.masterList, "viewList");
        spyOn(customerListIndex.agentList, "viewAgents");
        spyOn(customerListIndex.memberList, "viewMembers");
        spyOn(customerListIndex.agentList, "initAutoComplete");
        spyOn(customerListIndex.memberList, "initAutoComplete");

        spyOn(customerListIndex.agentList, "viewAgentsOfMaster");
        spyOn(customerListIndex.memberList, "viewMembersOfAgent");

        spyOn(topWindow.popupManager, "open");
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);

            // act
            customerListIndex.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(customerListIndex.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Init Tabs", () => {
            // assert
            expect(customerListIndex.masterList.init).toHaveBeenCalled();
            expect(customerListIndex.agentList.init).toHaveBeenCalled();
            expect(customerListIndex.memberList.init).toHaveBeenCalled();
        });
        it("_Bind change tab event", () => {
            // act
            $(".lnk-changetab").click();

            // assert
            expect(customerListIndex.changeTab).toHaveBeenCalled();
        });
        it("_Press enter in textbox", () => {
            // act
            // assert
            expect(true).toBeTruthy();
        });
        it("_Init Master List", () => {
            // arrange
            setFixtures(customerListIndexTemplate.masterTab);

            // act
            customerListIndex.init();

            // assert
            expect(customerListIndex.masterList.viewList).toHaveBeenCalled();
        });
        it("_Init Agent List", () => {
            // arrange
            setFixtures(customerListIndexTemplate.agentTab);

            // act
            customerListIndex.init();

            // assert
            expect(customerListIndex.agentList.viewAgents).toHaveBeenCalled();
        });
        it("_Init Member List", () => {
            // arrange
            setFixtures(customerListIndexTemplate.memberTab);

            // act
            customerListIndex.init();

            // assert
            expect(customerListIndex.memberList.viewMembers).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", () => {
        it("_All neccessary elements have been cached", () => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);

            // act
            customerListIndex.cacheDOMElements();

            // assert
            expect(customerListIndex.directDownline).toBeDefined();
        });
    });
    describe("_changeTab", () => {
        it("_To agent tab_InitAutoComplete", () => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);

            // act
            customerListIndex.changeTab(siteInfrastructure.roleId.agent.toString());

            // assert
            expect(customerListIndex.agentList.initAutoComplete).toHaveBeenCalled();
        });
        it("_To member tab_InitAutoComplete", () => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);

            // act
            customerListIndex.changeTab(siteInfrastructure.roleId.member.toString());

            // assert
            expect(customerListIndex.memberList.initAutoComplete).toHaveBeenCalled();
        });
    });
    describe("_getCustomerColor", () => {
        it("_ReturnRowOpenWithOpenAccount", () => {
            // act
            var actualValue = customerListIndex.getCustomerColor({ Status: siteInfrastructure.customerStatus.open });

            // assert
            expect(actualValue).toEqual("row-open");
        });
        it("_ReturnRowSuspendedWithSuspendedAccount", () => {
            // act
            var actualValue = customerListIndex.getCustomerColor({ Status: siteInfrastructure.customerStatus.suspended });

            // assert
            expect(actualValue).toEqual("row-suspended");
        });
        it("_ReturnRowClosedWithClosedAccount", () => {
            // act
            var actualValue = customerListIndex.getCustomerColor({ Status: siteInfrastructure.customerStatus.closed, CloseReason: 2 });

            // assert
            expect(actualValue).toEqual("row-closed reason-2");
        });
        it("_ReturnEmptyWithOtherStatus", () => {
            // act
            var actualValue = customerListIndex.getCustomerColor({ Status: siteInfrastructure.customerStatus.all });

            // assert
            expect(actualValue).toEqual("");
        });
    });

    describe("_updateCredit", () => {
        it("_open Popup", () => {
            // act
            customerListIndex.updateCredit(1, "Cust 1", 3);

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalled();
        });
    });
    describe("_updateStatus", () => {
        it("_open Popup", () => {
            // act
            customerListIndex.updateStatus(1, "Cust 1", 2);

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalled();
        });
    });
    describe("_getStatusName", () => {
        beforeEach(() => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);

            // act
            customerListIndex.init();
        });

        it("_ReturnOpenWithOpenStatus", () => {
            // act
            var actualValue = customerListIndex.getStatusName(siteInfrastructure.customerStatus.open);

            // assert
            expect(actualValue).toEqual("Open");
        });

        it("_ReturnClosedWithClosedStatus", () => {
            // act
            var actualValue = customerListIndex.getStatusName(siteInfrastructure.customerStatus.closed);

            // assert
            expect(actualValue).toEqual("Closed");
        });

        it("_ReturnSuspendedWithSuspendedStatus", () => {
            // act
            var actualValue = customerListIndex.getStatusName(siteInfrastructure.customerStatus.suspended);

            // assert
            expect(actualValue).toEqual("Suspended");
        });

        it("_ReturnEmptyWithOtherStatus", () => {
            // act
            var actualValue = customerListIndex.getStatusName(siteInfrastructure.customerStatus.all);

            // assert
            expect(actualValue).toEqual("");
        });
    });
    describe("_viewMaster", () => {
        it("_call viewAgentsOfMaster", () => {
            // act
            customerListIndex.viewMaster(1, "Cust 1");

            // assert
            expect(customerListIndex.agentList.viewAgentsOfMaster).toHaveBeenCalled();
        });
    });
    describe("_viewAgent", () => {
        it("_call viewMembersOfAgent", () => {
            // act
            customerListIndex.viewAgent(1, "Cust 1");

            // assert
            expect(customerListIndex.memberList.viewMembersOfAgent).toHaveBeenCalled();
        });
    });
    describe("_reloadStatusColor", () => {
        it("_ReturnRowOpenWithOpenAccount", () => {
            // act
            var actualValue = customerListIndex.reloadStatusColor(siteInfrastructure.customerStatus.open);

            // assert
            expect(actualValue).toEqual("row-open");
        });
        it("_ReturnRowSuspendedWithSuspendedAccount", () => {
            // act
            var actualValue = customerListIndex.reloadStatusColor(siteInfrastructure.customerStatus.suspended);

            // assert
            expect(actualValue).toEqual("row-suspended");
        });
        it("_ReturnRowClosedWithClosedAccount", () => {
            // act
            var actualValue = customerListIndex.reloadStatusColor(siteInfrastructure.customerStatus.closed);

            // assert
            expect(actualValue).toEqual("row-closed reason-2");
        });
    });
    describe("_reloadCreditValue", () => {
        it("_Update new value to element", () => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);
            customerListIndex.init();

            // act
            customerListIndex.reloadCreditValue(1, 1000);

            // assert
            expect($(".update-credit").html()).toEqual("1,000.00");
        });
    });
    describe("_reloadStatusValue", () => {
        it("_Update new value to element", () => {
            // arrange
            setFixtures(customerListIndexTemplate.normalMode);
            customerListIndex.init();

            // act
            customerListIndex.reloadStatusValue(1, 2);

            // assert
            expect($(".update-status").html()).toEqual("Suspended");
        });
    });
});