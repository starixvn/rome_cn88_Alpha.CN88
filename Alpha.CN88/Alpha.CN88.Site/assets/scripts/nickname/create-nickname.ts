"use strict";
class NickName {
    $: JQueryStatic;
    $nickName: JQuery;
    $messsageContainer: JQuery;
    nickNameRule1: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        let self: any = this;

        this.$nickName = this.$("#nickName");
        this.$nickName.focus();
        this.$messsageContainer = this.$("#message-container");
        this.nickNameRule1 = this.$("#nickNameRule1").html();

        this.$("#frmNickName").submit(function () {
            const nickNameRule = new RegExp(/^[a-zA-Z0-9]{5,15}$/i);
            const isMatch: boolean = nickNameRule.test(self.$.trim(self.$nickName.val()));

            if (!isMatch) {
                siteInfrastructure.showError(self.$messsageContainer, self.nickNameRule1);
                self.$nickName.focus();
            }

            return isMatch;
        });

        this.$("#buttonCancel").click(siteInfrastructure.logout);
    };
}

$(function () {
    new NickName($).init();
});