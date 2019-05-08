"use strict";
class Help {
    $: JQueryStatic;
    $backToTop: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.$backToTop = this.$("#back-to-top");

        this.$backToTop.on("click touchend", () => {
            $("html,body").animate({ scrollTop: 0 }, 1000);
        });
    }
}

$(function (): void {
    new Help($).init();
});