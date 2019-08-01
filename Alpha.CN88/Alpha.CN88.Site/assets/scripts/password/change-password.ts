"use strict";
class ChangePassword {
    $: JQueryStatic;
    top: Window;
    resources: {
        pleaseEnterOldPassword: string,
        passwordCannotEmpty: string,
        confirmPasswordNotCorret: string,
        newPassMustbeDiffOldPass: string,
        passwordDoesNotMeetMinimunRequirement: string
    };
    elements: {
        currentPassword: JQuery,
        newPassword: JQuery,
        confirmPassword: JQuery,
        messsageContainer: JQuery,
        hashKey: string
    };
    minPasswordComplication = 2;

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();
        this.$("#formPassword").submit((): boolean => {
            this.$.blockUI();
            const $currentPassword: JQuery = this.elements.currentPassword;
            const $newPassword: JQuery = this.elements.newPassword;
            const $confirmPassword: JQuery = this.elements.confirmPassword;

            if (!this.validatePasswords($currentPassword, $newPassword, $confirmPassword)) {
                this.$.unblockUI();
                return false;
            }

            const hashKey = this.elements.hashKey;

            if ($currentPassword.length > 0) {
                const currentPwd: string = $currentPassword.val().encrypt(hashKey);
                $currentPassword.val(currentPwd);
            }

            const newPwd: string = $newPassword.val().encrypt(hashKey);
            $newPassword.val(newPwd);
            $confirmPassword.val(newPwd);

            return true;
        });

        this.$("#buttonCancel").click(() => {
            if (this.$("#mainRootPath").length > 0) {
                siteInfrastructure.logout();
            } else {
                this.top.indexManager.hideUserMobileDropdown();
                this.top.popupManager.close();
            }
        });

        const focusField: JQuery = this.elements.currentPassword.length > 0
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
        const isChangePasswordPage: boolean = currentPassword.length > 0;
        const currentPasswordValue: string = isChangePasswordPage ? currentPassword.val() : "";
        const newPasswordValue: string = newPassword.val();
        const confirmPasswordValue: string = confirmPassword.val();

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
        const whiteSpace: RegExp = /^\S*$/;
        const whiteSpaceCase: boolean = password.search(whiteSpace) !== -1;

        const regex1: RegExp = /^(?=.{6,12}$).*\d/i;
        const regex2: RegExp = /^(?=.{6,12}$).*[a-z]/;
        const regex3: RegExp = /^(?=.{6,12}$).*[A-Z]/;
        const regex4: RegExp = /^(?=.{6,12}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        const case1: number = password.search(regex1) !== -1 ? 1 : 0;
        const case2: number = password.search(regex2) !== -1 ? 1 : 0;
        const case3: number = password.search(regex3) !== -1 ? 1 : 0;
        const case4: number = password.search(regex4) !== -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= this.minPasswordComplication) && whiteSpaceCase;
    }
}

$(function (): void {
    const changePassword: ChangePassword = new ChangePassword($, top);
    changePassword.init();
});