"use strict";
declare var Sha1: any;
declare var Sha256: any;

class VerifySecurityCode {
    $: JQueryStatic;
    top: Window;
    resources: {
        securityCodeEmpty: string
    };
    elements: {
        securityCode: JQuery,
        messsageContainer: JQuery,
        salt: string,
        token: string
    };

    constructor($: JQueryStatic, top: Window) {
        this.$ = $;
        this.top = top;
    }

    init(): void {
        this.cacheElements();

        this.$("#formSecurityCode").submit((e: JQueryEventObject): void => {
            e.preventDefault();
            this.verifySecurityCode(e.target);
        });

        this.elements.securityCode.focus();
        this.top.popupManager.setLatestPopupTitle(this.$("#securityCodeTitle").val());
    }

    cacheElements(): void {
        this.resources = {
            securityCodeEmpty: this.$("#alert-securitycode-empty").val()
        };

        this.elements = {
            securityCode: this.$("#securityCode"),
            messsageContainer: this.$("#message-container"),
            salt: this.$("#securitySalt").val(),
            token: this.$("#securitycodeToken").val()
        };
    }

    verifySecurityCode(form: Element): void {
        this.$.blockUI();

        const $securityCode: JQuery = this.elements.securityCode;
        let securityCodeValue: string = $securityCode.val();

        if (securityCodeValue.length === 0) {
            this.$.unblockUI();
            siteInfrastructure.showError(this.elements.messsageContainer, this.resources.securityCodeEmpty);
            $securityCode.focus();

            return;
        }

        securityCodeValue = securityCodeValue.hash(this.elements.salt, this.elements.token);
        this.submitForm(form.attributes["method"].nodeValue, form.attributes["action"].nodeValue, securityCodeValue);
    }

    submitForm(method, url, securityCode: string): void {
        this.$.ajax({
            type: method,
            url: url,
            headers: this.setupAntiXSRF(),
            data: { securityCode: securityCode },
            dataType: "json",
            success: (data: CodeResponse) => {
                if (data.Code === 0) {
                    this.elements.securityCode.attr("disabled", "disabled");
                    this.$("#btnSubmit").remove();
                    this.top.popupManager.revertLatestPopupTitle();
                    this.afterVerifed();
                }
                else if (data.Code === AccountStatusCode.CLOSE) { // account is closed
                    alert(data.Message);
                    siteInfrastructure.logout();
                }
                else {
                    siteInfrastructure.showError(this.elements.messsageContainer, data.Message);
                    this.elements.securityCode.focus();
                }
            },
            error: function (xhr) {
                if (xhr.status === HttpStatusCode.UNAUTHORIZED) {
                    alert(siteInfrastructure.getInputElementvalue("errorMessage"));
                    siteInfrastructure.logout();
                }
            },
            complete: this.$.unblockUI
        });
    }

    setupAntiXSRF(): object {
        const token = this.$("input[name=__RequestVerificationToken]").val();
        const headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    afterVerifed(): void {
        const redirectUrl = this.getUrlVars()["redirectUrl"];
        if (redirectUrl) {
            if (this.detectDCSUrl(redirectUrl) && this.detectIE()) {
                this.top.indexManager.elements.mainFrame.load(this.top.indexManager.locationReload);
            }

            window.location.href = decodeURIComponent(redirectUrl);
            return;
        }

        if (this.top.popupManager
            && this.top.popupManager.popupOptions
            && this.top.popupManager.popupOptions[this.top.popupManager.popupOptions.length - 1]
            && this.top.popupManager.popupOptions[this.top.popupManager.popupOptions.length - 1].preUrl) {
            const preUrl = this.top.popupManager.popupOptions[this.top.popupManager.popupOptions.length - 1].preUrl;
            this.top[preUrl]();
            this.top.popupManager.close();
            return;
        }

        window.location.href = this.$("#mainRootPath").val();
    }

    getUrlVars() {
        let hash;
        const vars = [];
        const url = window.location.href;
        const hashes = url.slice(url.indexOf("?") + 1).split("&");

        for (let i = 0, max = hashes.length; i < max; i++) {
            hash = hashes[i].split("=");
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }

        return vars;
    }

    detectDCSUrl(url: string): boolean {
        return url.toLowerCase().indexOf("viewcn88customersetting") > -1;
    }

    detectIE(): boolean {
        const ua = window.navigator.userAgent;
        return ua.indexOf("MSIE ") > 0 || ua.indexOf("Trident/") > 0 || ua.indexOf("Edge/") > 0;
    }
}

$(function (): void {
    const verifySecurityCode: VerifySecurityCode = new VerifySecurityCode($, top);
    verifySecurityCode.init();
});