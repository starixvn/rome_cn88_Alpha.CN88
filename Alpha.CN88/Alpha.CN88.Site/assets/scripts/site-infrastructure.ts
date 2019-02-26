declare var numeral: any;

interface JQuery {
    stickyTableHeaders(): void;
}

interface Window {
    cn88SuccessfulUpdatingCallback(): void;
    cn88CancelCallback(): void;
    $: JQueryStatic;
}

class Result {
    static warningCode: number = -1000;

    Code: number;
    Message: string;
    IsSuccessful: boolean;
    IsFailed: boolean;
    IsWarning: boolean;

    constructor(data?: any) {
        this.Code = data.Code;
        this.Message = data.Message || "";
        this.IsSuccessful = this.Code === 0;
        this.IsFailed = !this.IsSuccessful;
        this.IsWarning = data.Code === Result.warningCode;
    }
}

class SiteInfrastructure {
    localStorage = {
        get: (key: string) => {
            var result = store.enabled ? store.get(key) : $.cookie(key);
            return result ? JSON.parse(result) : undefined;
        },

        set: (key: string, value: any) => {
            var data = JSON.stringify(value);
            store.enabled ? store.set(key, data) : $.cookie(key, data, this.localStorage.getCookieExpireOptions());
        },

        check: (key: string): boolean => {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            return cookieExpire;
        },
        clear: () => {
            if (store.enabled) {
                store.clear();
            }
        }
    };

    loadInfo = ($: any, id: string, handleOnSuccessLoading: any) => {
        let $component = $(id),
            url = $component.attr('action-url');

        if (url == '' || typeof url == 'undefined') {
            return;
        }

        $.ajax({
            method: 'GET',
            url: url,
            cache: false,
            success: (responseHtml) => {
                $component.html(responseHtml);
                handleOnSuccessLoading();
            }
        });
    };

    showMessage = ($target: any, result: Result) => {
        $target.empty();

        if (result.IsSuccessful) {
            this.showSuccess($target, result.Message);
        } else if (result.IsFailed) {
            this.showError($target, result.Message);
        } else if (result.IsWarning) {
            this.showWarn($target, result.Message);
        }
    };

    showSuccess = ($target: any, message: string) => {
        $target.html("<div class='msg msg-success'>" + message + "</div>");
    };

    showError = ($target: any, error: string) => {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    };

    showWarn = ($target: any, warning: string) => {
        $target.html("<div class='msg msg-warning'>" + warning + "</div>");
    };

    logout = () => {
        var index: any = window;
        while (index != index.parent) {
            index = index.parent;
        }

        const mainRootPath = (<HTMLInputElement>top.document.getElementById("mainRootPath")).value;
        const isInternalElement = (<HTMLInputElement>top.document.getElementById("IsInternal"));
        const isInternal = isInternalElement ? isInternalElement.value : "0";

        index.location.href = mainRootPath + "Auth/Logout";
        siteInfrastructure.localStorage.clear();

        if (isInternal === "1") {
            index.close();
        }
    };

    openPopup = (popupTitle: string, url: string, height: number = 360) => {
        const calculatePopupHeight = function (height: number) {
            const headerHeight = 40;
            const contentBorder = 2;
            let popupHeight = height || 360;
            popupHeight += 50;

            return popupHeight - headerHeight - contentBorder;
        };

        top.$("#mainPopup").on("show.bs.modal", function () {
            const modal = top.$(this);
            modal.find(".modal-title").text(popupTitle);
            modal.find(".modal-content .modal-body").css("height", calculatePopupHeight(height) + "px");
            modal.find("iframe").attr("src", url);
        }).on("hidden.bs.modal", function () {
            const modal = top.$(this);
            modal.find("iframe").attr("src", "about:blank");
        }).modal("show");
    };

    closePopup = () => {
        top.$("#mainPopup").modal("hide");
    };
}

const siteInfrastructure = new SiteInfrastructure();

window.top.cn88SuccessfulUpdatingCallback = function () {
    var $currentPage = top.$("body #reloadCN88CustomerListCallBackUrl");

    if ($currentPage.length == 0 || $currentPage.val() == "") {
        return;
    }

    top.$("#main").attr("src", $currentPage.val());
}

//window.top.cn88CancelCallback = function () {
//	var $currentPage = window.top.$("body #reloadCN88CustomerListCallBackUrl");

//	if ($currentPage.length == 0 || $currentPage.val() == "") {
//		return;
//	}

//	top.$("#main").attr("src", $currentPage.val());
//}