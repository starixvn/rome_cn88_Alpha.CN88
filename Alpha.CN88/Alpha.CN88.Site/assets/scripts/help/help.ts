"use strict";
class Help {
    $: JQueryStatic;
    $backToTop: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.$backToTop = this.$("#back-to-top");
        this.$backToTop.hide();

        let self = this;
        $(window).scroll(function () {
            if ($(this).scrollTop() > 50) {
                self.$backToTop.fadeIn();
            } else {
                self.$backToTop.fadeOut();
            }
        });

        this.$backToTop.on("click touchend", () => {
            $("html,body").animate({ scrollTop: 0 }, 500);
        });
    }
}

$(function (): void {
    new Help($).init();
});