declare var numeral: any;
interface JQuery {
    stickyTableHeaders(): void;
}

class SiteInfrastructure {
    pageList = [50, 100, 200, 500];

    customerStatus = {
        all: 0,
        open: 1,
        closed: 3,
        suspended: 2
    };

    closeReasonConstant = {
        LogInFailedOrExpired: 0,
        SecurityCode: 3,
        Upline: 2,
        Admin: 1
    };

    roleId = {
        master: 3,
        agent: 2,
        member: 1
    };

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