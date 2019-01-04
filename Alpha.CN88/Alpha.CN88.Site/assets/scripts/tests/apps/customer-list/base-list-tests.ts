describe("BaseList", () => {
    const template = {
        normalMode: `<body>
                        <div id="tbl-member">
                            <div class="view-downline"></div>
                            <div class="update-credit"></div>
                            <div class="update-status"></div>
                            <div class="icon-edit"></div>
                            <div class="icon-add"></div>
                        </div>
                        <div id="btn-submit-member"></div>
                        <input id="HasUpdatePermission" value="1" />
                        <input id="HasAddNewPermission" value="1" />
                    </body>`
    };

    let baseList: BaseList;
    const statusComponent = new SingleSelectionDropdownComponent($);
    const pager = new PagerComponent($);
    const topWindow = { popupManager: { open: function (url, title) { } } };
    let renderList: Function;

    beforeEach(() => {
        baseList = new BaseList($, statusComponent, numeral, pager, "Member");
        spyOn(baseList, "cacheDOMElements").and.callThrough();
        spyOn(statusComponent, "render");
        spyOn(baseList, "viewList").and.callThrough();
        spyOn(baseList, "loadData").and.callThrough();
        spyOn($, "blockUI");
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);

            // act
            baseList.init({}, null, jasmine.any(Function), 1, jasmine.any(Function), jasmine.any(Function));
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(baseList.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Renders status dropdown", () => {
            // assert
            expect(statusComponent.render).toHaveBeenCalled();
        });
        it("_Bind submit event", () => {
            // act
            $("#btn-submit-member").click();

            // assert
            expect(baseList.viewList).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", () => {
        it("_All neccessary elements have been cached", () => {
            // arrange
            setFixtures(template.normalMode);

            // act
            baseList.cacheDOMElements();

            // assert
            expect(baseList.$tblReport).toBeDefined();
        });
    });
    describe("_viewList", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            baseList.init({}, null, renderList, 1, jasmine.any(Function), jasmine.any(Function));

            // act
            baseList.viewList();
        });

        it("_call blockUI", () => {
            // assert
            expect(baseList.$.blockUI).toHaveBeenCalled();
        });
    });
    describe("_pagingList", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normalMode);
            baseList.init({}, null, renderList, 1, jasmine.any(Function), jasmine.any(Function));

            // act
            baseList.pagingList();
        });

        it("_call loadData", () => {
            // assert
            expect(baseList.loadData).toHaveBeenCalled();
        });
    });

    describe("_bindReportEvents ", () => {
        it("_bind view downline event", () => {
            // arrange
            setFixtures(template.normalMode);
            baseList.init({}, null, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(baseList, "viewDownlineFunction");

            // act
            baseList.bindReportEvents({});
            $(".view-downline").click();

            // assert
            expect(baseList.viewDownlineFunction).toHaveBeenCalled();
        });
        it("_bind update credit event", () => {
            // arrange
            setFixtures(template.normalMode);
            var indexManager = {
                updateCredit: jasmine.any(Function)
            };

            baseList.init({}, indexManager, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(indexManager, "updateCredit");

            // act
            baseList.bindReportEvents({});
            $(".update-credit").click();

            // assert
            expect(indexManager.updateCredit).toHaveBeenCalled();
        });
        it("_bind update status event", () => {
            // arrange
            setFixtures(template.normalMode);
            var indexManager = {
                updateStatus: jasmine.any(Function)
            };

            baseList.init({}, indexManager, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(indexManager, "updateStatus");

            // act
            baseList.bindReportEvents({});
            $(".update-status").click();

            // assert
            expect(indexManager.updateStatus).toHaveBeenCalled();
        });
        it("_bind update account event", () => {
            // arrange
            setFixtures(template.normalMode);
            var indexManager = {
                updateAccount: jasmine.any(Function)
            };

            baseList.init({}, indexManager, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(indexManager, "updateAccount");

            // act
            baseList.bindReportEvents({});
            $(".icon-edit").click();

            // assert
            expect(indexManager.updateAccount).toHaveBeenCalled();
        });
        it("_bind add downline event", () => {
            // arrange
            setFixtures(template.normalMode);
            var indexManager = {
                addDownlineFunction: jasmine.any(Function)
            };

            baseList.init({}, indexManager, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(baseList, "addDownlineFunction");

            // act
            baseList.bindReportEvents({});
            $(".icon-add").click();

            // assert
            expect(baseList.addDownlineFunction).toHaveBeenCalled();
        });
        it("_render pager", () => {
            // arrange
            setFixtures(template.normalMode);
            var indexManager = {
                addDownlineFunction: jasmine.any(Function)
            };

            baseList.init({}, indexManager, renderList, 1, jasmine.any(Function), jasmine.any(Function));
            spyOn(baseList.pager, "render");

            // act
            baseList.bindReportEvents({ TotalCustomers: 51 });

            // assert
            expect(baseList.pager.render).toHaveBeenCalled();
        });
    });

});