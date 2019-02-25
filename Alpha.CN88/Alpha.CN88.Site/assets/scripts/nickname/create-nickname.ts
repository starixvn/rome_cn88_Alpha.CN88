"use strict";
class NickName {
    $: JQueryStatic;
    $nickName: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        let self = this;
        this.$nickName = this.$("#nickname");
        this.$nickName.focus();

        this.$("#frmNickName").submit(function () {
            const nickNameRule = new RegExp(/^[a-zA-Z0-9]{5,15}$/i);
            var isMatch = nickNameRule.test(self.$.trim(self.$nickName.val()));

            if (!isMatch) {
                alert("Nickname must be unique, only alphabet or numeric and should have at least a minimum of 5 characters and maximum of 15 characters.");
                self.$nickName.focus();
            }

            return isMatch;
        });
    };
}

$(function () {
    new NickName($).init();
});