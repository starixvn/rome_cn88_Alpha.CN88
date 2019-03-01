class HomeIndex {
    protected $: JQueryStatic;
    protected topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        siteInfrastructure.loadInfo(this.$, '#announcements', this.initAnnouncements);
    };

    initAnnouncements = () => {
        var self = this;

        this.$("#view-announcements").on("click touchend", function () {
            self.topWindow.indexManager.activeMainMenu("menu-ANNOUNCEMENT");

            var target = self.$(this).attr("data-target");

            self.topWindow.$("#main").attr("src", target);
        });
    };
};


$(function () {
    var homeIndex = new HomeIndex($, top);
    homeIndex.init();
});