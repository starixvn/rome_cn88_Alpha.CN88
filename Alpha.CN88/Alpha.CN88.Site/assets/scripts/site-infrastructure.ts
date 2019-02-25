declare var numeral: any;

interface JQuery {
	stickyTableHeaders(): void;
}

interface Window {
	cn88SuccessfulUpdatingCallback(): void;
	cn88CancelCallback(): void;
	$: JQueryStatic;
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
    }
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