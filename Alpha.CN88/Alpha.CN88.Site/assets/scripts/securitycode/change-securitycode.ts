"use strict";
class ChangeSecurityCode {
    readonly codeLength = 6;

    $: JQueryStatic;
    top: Window;
    resources: {
        oldSecurityCodeEmpty: string,
        securityCodeEmpty: string,
        securityCodeWrongRule: string,
        errorSecurityCodeConfirm: string,
        sameSecurityCode: string
    };
    elements: {
        oldSecurityCode: JQuery,
        securityCode: JQuery,
        confirmSecurityCode: JQuery,
        messsageContainer: JQuery,
        hashKey: string,
        salt: string,
        token: string
    };

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();

        this.$("#formSecurityCode").submit((): boolean => {
            this.$.blockUI();
            const $oldSecurityCode: JQuery = this.elements.oldSecurityCode;
            const $securityCode: JQuery = this.elements.securityCode;
            const $confirmSecurityCode: JQuery = this.elements.confirmSecurityCode;

            if (!this.validateSecurityCodes($oldSecurityCode, $securityCode, $confirmSecurityCode)) {
                this.$.unblockUI();
                return false;
            }

            const hashKey = this.elements.hashKey;
            if ($oldSecurityCode.length > 0) {
                const oldSecCode: string = $oldSecurityCode.val().hash(this.elements.salt, this.elements.token);
                $oldSecurityCode.val(oldSecCode);
            }

            const secCode: string = $securityCode.val().encrypt(hashKey);
            $securityCode.val(secCode);
            $confirmSecurityCode.val(secCode);

            return true;
        });

        this.$("#buttonCancel").click((): void => {
            if (this.$("#mainRootPath").length > 0) {
                siteInfrastructure.logout();
            } else {
                this.top.indexManager.hideUserMobileDropdown();
                this.top.popupManager.close();
            }
        });

        const focusField: JQuery = this.elements.oldSecurityCode.length > 0
            ? this.elements.oldSecurityCode
            : this.elements.securityCode;
        focusField.focus();
    }

    cacheElements(): void {
        this.resources = {
            oldSecurityCodeEmpty: this.$("#alert-old-securitycode-empty").val(),
            securityCodeEmpty: this.$("#alert-securitycode-empty").val(),
            securityCodeWrongRule: this.$("#alert-securitycode-wrong-rule").val(),
            errorSecurityCodeConfirm: this.$("#alert-error-securitycode-confirm").val(),
            sameSecurityCode: this.$("#alert-same-securitycode").val()
        };

        this.elements = {
            oldSecurityCode: this.$("#oldSecurityCode"),
            securityCode: this.$("#securityCode"),
            confirmSecurityCode: this.$("#confirmSecurityCode"),
            messsageContainer: this.$("#message-container"),
            hashKey: this.$("#hashKey").val(),
            salt: this.$("#securitySalt").val(),
            token: this.$("#securitycodeToken").val()
        };
    }

    validateSecurityCodes(oldSecurityCode: JQuery, securityCode: JQuery, confirmSecurityCode: JQuery): boolean {
        const isChangeSecurityCodePage: boolean = oldSecurityCode.length > 0;
        const oldSecurityCodeValue: string = isChangeSecurityCodePage ? oldSecurityCode.val() : "";
        const securityCodeValue: string = securityCode.val();
        const confirmSecurityCodeValue: string = confirmSecurityCode.val();

        if (isChangeSecurityCodePage && oldSecurityCodeValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.oldSecurityCodeEmpty);
            oldSecurityCode.focus();

            return false;
        }

        if (securityCodeValue.length === 0) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeEmpty);
            securityCode.focus();

            return false;
        }

        if (!this.isValidSecurityCode(securityCodeValue)) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeWrongRule);
            securityCode.focus();

            return false;
        }

        if (securityCodeValue !== confirmSecurityCodeValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.errorSecurityCodeConfirm);
            confirmSecurityCode.focus();

            return false;
        }

        if (isChangeSecurityCodePage && securityCodeValue === oldSecurityCodeValue) {
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.sameSecurityCode);
            securityCode.focus();

            return false;
        }

        return true;
    }

    isValidSecurityCode(securityCode: string): boolean {
        const securityCodeLength: number = securityCode ? securityCode.length : 0;
        if (securityCodeLength !== this.codeLength) {
            return false;
        }

        const regex: RegExp = /^([0-9])(?!\1+$)[0-9]+$/;
        if (securityCode.search(regex) === -1) {
            return false;
        }

        let isForwardRun = true;
        let isBackwardRun = true;
        let forward = parseInt(securityCode.charAt(0));
        let backward = forward - 1;
        for (let i = 1; i < securityCodeLength; i++) {
            forward++;
            backward--;
            const current: number = parseInt(securityCode.charAt(i));
            isForwardRun = isForwardRun && (forward === current);
            isBackwardRun = isBackwardRun && (backward === current);
        }

        return !(isForwardRun || isBackwardRun);
    }
}

$(function (): void {
    const changeSecurityCode: ChangeSecurityCode = new ChangeSecurityCode($, top);
    changeSecurityCode.init();
});