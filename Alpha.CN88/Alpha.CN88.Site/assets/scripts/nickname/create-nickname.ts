"use strict";
class NickName {
    $: JQueryStatic;
    $nickName: JQuery;
    $messsageContainer: JQuery;
    nickNameRule1: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.$nickName = this.$("#nickName");
        this.$nickName.focus();
        this.$messsageContainer = this.$("#message-container");
        this.nickNameRule1 = this.$("#nickNameRule1").html();

        this.$("#frmNickName").submit((): boolean => {
            this.$.blockUI();
            const nickNameRule: RegExp = new RegExp(/^[a-zA-Z0-9]{5,20}$/i);
            const isMatch: boolean = nickNameRule.test(this.$.trim(this.$nickName.val()));

            if (!isMatch) {
                this.$.unblockUI();
                siteInfrastructure.showError(this.$messsageContainer, this.nickNameRule1);
                this.$nickName.focus();
            }

            return isMatch;
        });

        this.$("#buttonBack").click(siteInfrastructure.logout);
    }
}

$(function (): void {
    new NickName($).init();
});