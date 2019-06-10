"use strict";
class ChangePassword {
    $: JQueryStatic;
    top: Window;
    resources: any;
    elements: any;

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        let self: any = this;
        this.cacheElements();

        this.$("#formPassword").submit(function (): boolean {
            self.$.blockUI();
            let $currentPassword: JQuery = self.elements.currentPassword;
            let $newPassword: JQuery = self.elements.newPassword;
            let $confirmPassword: JQuery = self.elements.confirmPassword;

            if (!self.validatePasswords($currentPassword, $newPassword, $confirmPassword)) {
                self.$.unblockUI();
                return false;
            }

            let hashKey: string = self.elements.hashKey;

            if ($currentPassword.length > 0) {
                let currentPwd: string = $currentPassword.val().encrypt(hashKey);
                $currentPassword.val(currentPwd);
            }

            let newPwd: string = $newPassword.val().encrypt(hashKey);
            $newPassword.val(newPwd);
            $confirmPassword.val(newPwd);

            return true;
        });

        this.$("#buttonCancel").click(() => {
            if (self.$("#mainRootPath").length > 0) {
                siteInfrastructure.logout();
            } else {
                self.top.indexManager.hideUserMobileDropdown();
                self.top.popupManager.close();
            }
        });

        let focusField: JQuery = this.elements.currentPassword.length > 0
            ? this.elements.currentPassword
            : this.elements.newPassword;
        focusField.focus();
    }

    cacheElements(): void {
        this.resources = {
            pleaseEnterOldPassword: this.$("#please-enter-oldPassword").val(),
            passwordCannotEmpty: this.$("#password-cannot-empty").val(),
            confirmPasswordNotCorret: this.$("#not-correct-confirm-password").val(),
            newPassMustbeDiffOldPass: this.$("#newPassword-must-be-diff-old").val(),
            passwordDoesNotMeetMinimunRequirement: this.$("#password-does-not-min-requirement").val()
        };

        this.elements = {
            currentPassword: this.$("#currentPassword"),
            newPassword: this.$("#newPassword"),
            confirmPassword: this.$("#confirmPassword"),
            messsageContainer: this.$("#message-container"),
            hashKey: this.$("#hashKey").val()
        };
    }

    validatePasswords(currentPassword: JQuery, newPassword: JQuery, confirmPassword: JQuery): boolean {
        let isChangePasswordPage: boolean = currentPassword.length > 0;
        let currentPasswordValue: string = isChangePasswordPage ? currentPassword.val() : "";
        let newPasswordValue: string = newPassword.val();
        let confirmPasswordValue: string = confirmPassword.val();

        if (isChangePasswordPage && currentPasswordValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.pleaseEnterOldPassword);
            currentPassword.focus();

            return false;
        }

        if (newPasswordValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.passwordCannotEmpty);
            newPassword.focus();

            return false;
        }

        if (!this.isValidPassword(newPasswordValue)) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.passwordDoesNotMeetMinimunRequirement);
            newPassword.focus();

            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.confirmPasswordNotCorret);
            confirmPassword.focus();

            return false;
        }

        if (isChangePasswordPage && newPasswordValue === currentPasswordValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.newPassMustbeDiffOldPass);
            newPassword.focus();

            return false;
        }

        return true;
    }

    isValidPassword(password: string) {
        let whiteSpace: RegExp = /^\S*$/;
        let whiteSpaceCase: boolean = password.search(whiteSpace) !== -1;

        let regex1: RegExp = /^(?=.{6,12}$).*\d/i;
        let regex2: RegExp = /^(?=.{6,12}$).*[a-z]/;
        let regex3: RegExp = /^(?=.{6,12}$).*[A-Z]/;
        let regex4: RegExp = /^(?=.{6,12}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        let case1: number = password.search(regex1) !== -1 ? 1 : 0;
        let case2: number = password.search(regex2) !== -1 ? 1 : 0;
        let case3: number = password.search(regex3) !== -1 ? 1 : 0;
        let case4: number = password.search(regex4) !== -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= 2) && whiteSpaceCase;
    }
}

$(function (): void {
    const changePassword: ChangePassword = new ChangePassword($, top);
    changePassword.init();
});