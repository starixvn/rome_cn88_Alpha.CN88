class HomeIndex {
    protected $: JQueryStatic;
    protected topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        siteInfrastructure.loadInfo(this.$, '#important-announcement', this.initImportantAnnouncement);
        siteInfrastructure.loadInfo(this.$, '#general-announcement', this.initGeneralAnnouncement);
    };

    initImportantAnnouncement = () => {
        var self = this;

        this.$("#view-important-announcement").on("click touchend", function () {
            self.topWindow.$("#menu-ANNOUNCEMENT").click();
        });
    };

    initGeneralAnnouncement = () => {
        var self = this;

        this.$("#view-genral-announcement").on("click touchend", function () {
            self.topWindow.$("#menu-ANNOUNCEMENT").click();
        });
    };
};


$(function () {
    var homeIndex = new HomeIndex($, top);
    homeIndex.init();
});