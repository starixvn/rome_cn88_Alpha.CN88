class HomeIndex {
    protected $: JQueryStatic;
    protected topWindow: any;

    markMessageAsReadPath = null;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.markMessageAsReadPath = this.$("#MarkMessageAsReadPath").val();
        siteInfrastructure.loadInfo(this.$, "#announcements", this.initAnnouncements);
    };

    initAnnouncements = () => {
        var self = this;

        this.onToggleMessage(this.markMessageAsRead);
        this.$("#view-announcements").on("click touchend", function () {
            self.topWindow.indexManager.activeMainMenu("menu-ANNOUNCEMENT");

            var target = self.$(this).attr("data-target");

            self.topWindow.$("#main").attr("src", target);
        });
    };

    onToggleMessage = (markMessageAsRead: (message: JQuery) => void) => {
        this.$(".message").each((index: number, element: Element) => {
            let $this = this.$(element),
                shortContent = $this.children(".short-content"),
                fullContent = $this.children(".full-content"),
                icon = $this.find(".icon");

            $this.on("click", () => {
                if (!shortContent.is(":hidden")) {
                    shortContent.hide();
                    fullContent.show();
                    icon.toggleClass("icon-plus icon-minus");
                    markMessageAsRead($this);
                }
                else {
                    shortContent.show();
                    fullContent.hide();
                    icon.toggleClass("icon-minus icon-plus");
                }
            });
        });
    }
    markMessageAsRead = (message: JQuery) => {
        const isUnRead: boolean = message.hasClass("bold");

        if (isUnRead) {
            const onSuccess = (isMarkedSuccess: any) => {
                if (isMarkedSuccess) {
                    message.removeClass("bold");
                }
            };

            this.$.ajax({
                url: this.markMessageAsReadPath,
                data: {
                    messageId: message.attr("id")
                },
                method: "POST",
                success: onSuccess
            });
        }
    }
};


$(function () {
    var homeIndex = new HomeIndex($, top);
    homeIndex.init();
});