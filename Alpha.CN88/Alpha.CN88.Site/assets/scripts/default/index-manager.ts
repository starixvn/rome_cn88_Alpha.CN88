var indexManager = {
    urls: {
        mainRootPath: null,
        changeLanguage: null,
        onlineList: null,
        liveChat: null,
        contactUs: null
    },
    elements: {
        mainMenu: null,
        subMenu: null,
        currentURLCookieName: null,
        subMenuItems: null,
        mainFrame: null
    },
    currentState: {
        mainMenuId: null
    },
    init: function () {
        indexManager.cacheElements();
        indexManager.bindEvents();

        indexManager.restoreLastMenu();
    },
    cacheElements: function () {
        indexManager.urls.mainRootPath = top.document.getElementById('mainRootPath') != null
            ? (<HTMLInputElement>top.document.getElementById('mainRootPath')).value
            : "not found";

        indexManager.urls.changeLanguage = indexManager.urls.mainRootPath + "Language/Change";
        indexManager.urls.onlineList = indexManager.urls.mainRootPath + "CustomerList/OnlineMember";
        indexManager.urls.liveChat = indexManager.urls.mainRootPath + "CustomerList/LiveChat";
        indexManager.urls.contactUs = indexManager.urls.mainRootPath + "Help/ContactUs";

        indexManager.elements.mainMenu = $(".menu-item");
        indexManager.elements.subMenu = $(".sub-menu");
        indexManager.elements.subMenuItems = $(".sub-menu-item");
        indexManager.elements.currentURLCookieName = $("#currentURLCookieName").val();
        indexManager.elements.mainFrame = $("#main");
    },
    bindEvents: function () {
        $(".changelanguage-link").on("click touchend", (event) => {
            indexManager.changeLanguage($(event.target).data("value"));
        });
        $(".online-member").on("click touchend", () => {
            indexManager.viewOnlineList();
        });
        indexManager.elements.mainMenu.on("click touchend", function () {
            var $this = $(this);
            indexManager.mainMenuClick($this.attr("id"));
        });
        indexManager.elements.subMenuItems.on("click touchend", function () {
            var $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
        });

        $("#lnk-new-url").on("click touchend", () => {
            $("#menu-HELP").click();
        });

        $("#lnk-live-chat").on("click touchend", () => {
            window.open(indexManager.urls.liveChat, "", "height=600,width=800");
        });
        $("#lnk-personal-announcement").on("click touchend", () => {
            indexManager.elements.mainMenu.removeClass("active");
            var id = "menu-ANNOUNCEMENT";
            $("#" + id).addClass("active");

            indexManager.currentState.mainMenuId = id;
            indexManager.elements.subMenu.addClass("display-none");
            $(".sub-menu[data-main-menu='" + id + "']").removeClass("display-none");
            $("#sub-menu-PERSONALANNOUNCEMENT")[0].click();
        });
        $("#lnk-contact-us").on("click touchend", () => {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.addClass("display-none");

            indexManager.addHistory({ mainMenu: null, subMenu: null });
            indexManager.elements.mainFrame.attr("src", indexManager.urls.contactUs);
        });
        $("#menu-back").on("click touchend", () => {
            var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
            if (!history || history.length == 0) {
                return;
            }

            history.pop();
            siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, history);
            indexManager.restoreLastMenu();
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
        window.open(indexManager.urls.onlineList, "", "height=600,width=800");
    },
    mainMenuClick: (id) => {
        indexManager.elements.mainMenu.removeClass("active");
        var $this = $(id);

        $this.addClass("active");

        indexManager.currentState.mainMenuId = id;
        indexManager.elements.subMenu.addClass("display-none");

        var subMenu = $(".sub-menu[data-main-menu='" + id + "']");
        if (subMenu && subMenu.length) {
            subMenu.removeClass("display-none");
            subMenu.find(".sub-menu-item >.redirect-item:first")[0].click();
        }
    },
    subMenuClick: (id) => {
        indexManager.elements.subMenuItems.removeClass("active");
        var $this = $(id);

        $this.addClass("active");

        indexManager.addHistory({ mainMenu: indexManager.currentState.mainMenuId, subMenu: id });
    },
    addHistory: (newEvent) => {
        var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        if (!history) {
            history = [];
        }

        history.push(newEvent);
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, history);
    },
    restoreLastMenu: () => {
        var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        if (!history || history.length == 0) {
            return;
        }

        var currentMenu = history.pop();
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, history);

        if (currentMenu.mainMenu == null) {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.addClass("display-none");
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        $("#" + currentMenu.mainMenu).addClass("active");

        indexManager.currentState.mainMenuId = currentMenu.mainMenu;
        var subMenu = $(".sub-menu[data-main-menu='" + currentMenu.mainMenu + "']");
        if (subMenu && subMenu.length) {
            subMenu.find("#" + currentMenu.subMenu)[0].click();
        } else {
            $("#" + currentMenu.mainMenu).click();
        }
    }
};

$(function () {
    indexManager.init();
});