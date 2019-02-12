var indexManager = {
    urls: {
        mainRootPath: null,
        changeLanguage: null,
        onlineList: null
    },
    init: function () {
        indexManager.cacheElements();
        indexManager.bindEvents();
    },
    cacheElements: function () {
        indexManager.urls.mainRootPath = top.document.getElementById('mainRootPath') != null
            ? (<HTMLInputElement>top.document.getElementById('mainRootPath')).value
            : "not found";

        indexManager.urls.changeLanguage = indexManager.urls.mainRootPath + "Language/Change";
        indexManager.urls.onlineList = indexManager.urls.mainRootPath + "CustomerList/OnlineMember";
    },
    bindEvents: function () {
        $(".changelanguage-link").on('click touchend', (event) => {
            indexManager.changeLanguage($(event.target).data("value"));
        });
        $(".online-member").on('click touchend', () => {
            indexManager.viewOnlineList();
        });
    },
    changeLanguage: (lang) => {
        $.ajax({
            type: "POST",
            url: indexManager.urls.changeLanguage,
            data: { languageCode: lang },
            success: function (result) {
                if (result !== undefined) {
                    // Remove www so that subdomain can override LANGUAGE value
                    var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace("www.", '');
                    $.cookie("LANGUAGE", lang, { expires: 30, path: domain });
                    indexManager.reloadPage();
                }
            }
        });
    },
    reloadPage: () => {
        top.location.href = indexManager.removeBookmarksInUrl(top.location.href);
    },
    removeBookmarksInUrl: (url) => {
        if (typeof url !== 'string') {
            return null;
        }

        var i = url.indexOf("#");
        if (i === -1) {
            return url;
        }

        return url.substr(0, i);
    },
    viewOnlineList: () => {
        window.open(indexManager.urls.onlineList, "", "height=600,width=800")
    }
};

$(function () {
    indexManager.init();
});