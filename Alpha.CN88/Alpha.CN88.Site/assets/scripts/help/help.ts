"use strict";
class Help {
    $: JQueryStatic;
    $backToTop: JQuery;
    topMaxHeight = 50;
    animateDuration = 500;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.$backToTop = this.$("#back-to-top");
        this.$backToTop.hide();

        $(window).scroll(() => {
            if ($(window).scrollTop() > this.topMaxHeight) {
                this.$backToTop.fadeIn();
            } else {
                this.$backToTop.fadeOut();
            }
        });

        this.$backToTop.on("click touchend", () => {
            $("html,body").animate({ scrollTop: 0 }, this.animateDuration);
        });
    }
}

$(function (): void {
    new Help($).init();
});