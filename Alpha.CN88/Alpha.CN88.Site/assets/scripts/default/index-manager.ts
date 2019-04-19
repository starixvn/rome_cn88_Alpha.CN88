var indexManager = {
    urls: {
        mainRootPath: null,
        changeLanguage: null,
        getTotalOnlineMemberUrl: null,
        onlineList: null,
        liveChat: null,
        contactUs: null,
        reCheckin: null,
        changePassword: null,
        changeSecurityCode: null,
        onlineMembersPath: null,
        oldReportsPath: null
    },
    elements: {
        mainMenu: null,
        subMenu: null,
        currentURLCookieName: null,
        subMenuItems: null,
        mainFrame: null,
        menuBar: null,
        onlineUserId: null,
        clearClientData: null,
        isNotHeadLine: 1,
        intervalToGetTotalOnlineMember: 600000,
    },
    currentState: {
        mainMenuId: null,
        isOpenPopup: false
    },
    resources: {
        changePassword: null,
        changeSecurityCode: null
    },
    topWindow: null,
    init: function (topWindow: any) {
        indexManager.topWindow = topWindow;

        indexManager.cacheElements();
        indexManager.bindEvents();

        if (indexManager.elements.clearClientData == "1") {
            siteInfrastructure.localStorage.clear();
        }

        indexManager.restoreUrl();
    },
    cacheElements: function () {
        indexManager.elements.isNotHeadLine = parseInt($("#IsNotHeadLine").val());
        indexManager.elements.intervalToGetTotalOnlineMember = parseInt($("#IntervalToGetTotalOnlineMember").val());
        indexManager.urls.mainRootPath = top.document.getElementById('mainRootPath') != null
            ? (<HTMLInputElement>top.document.getElementById('mainRootPath')).value
            : "not found";

        indexManager.urls.changeLanguage = indexManager.urls.mainRootPath + "Language/Change";
        indexManager.urls.getTotalOnlineMemberUrl = indexManager.urls.mainRootPath + "Default/GetTotalOnlineMember";
        indexManager.urls.onlineList = $("#OnlineMembersPath").val();
        indexManager.urls.liveChat = indexManager.urls.mainRootPath + "CustomerList/LiveChat";
        indexManager.urls.contactUs = indexManager.urls.mainRootPath + "Help/ContactUs";
        indexManager.urls.reCheckin = indexManager.urls.mainRootPath + "ReCheckin";
        indexManager.urls.changePassword = indexManager.urls.mainRootPath + "Password/ChangePassword";
        indexManager.urls.changeSecurityCode = indexManager.urls.mainRootPath + "SecurityCode/ChangeSecurityCode";
        indexManager.urls.oldReportsPath = $("#OldReportsPath").val();

        indexManager.elements.mainMenu = $(".menu-item");
        indexManager.elements.subMenu = $(".sub-menu");
        indexManager.elements.subMenuItems = $(".sub-menu-item");
        indexManager.elements.currentURLCookieName = $("#currentURLCookieName").val();
        indexManager.elements.mainFrame = $("#main");
        indexManager.elements.menuBar = $(".menubar");
        indexManager.elements.onlineUserId = $("#OnlineUserId").val();
        indexManager.elements.clearClientData = $("#ClearClientData").val().toString();
        indexManager.resources.changePassword = $("#ChangePassword").val();
        indexManager.resources.changeSecurityCode = $("#ChangeSecurityCode").val();
    },
    bindEvents: function () {
        $(".changelanguage-link").on("click touchend", (event) => {
            indexManager.changeLanguage($(event.target).data("value"));
        });
        $(".online-member").on("click touchend", () => {
            indexManager.viewOnlineList();
        });
        indexManager.elements.mainMenu.on("click touchend", function () {
            $.blockUI();
            var $this = $(this);
            indexManager.mainMenuClick($this.attr("id"));
        });
        indexManager.elements.subMenuItems.on("click touchend", function () {
            $.blockUI();
            var $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
        });

        $("#lnk-new-url").on("click touchend", () => {
            $("#menu-HELP").click();
        });

        $("#lnk-live-chat").on("click touchend", () => {
            window.open(indexManager.urls.liveChat, "LiveChat", "height=600,width=800");
        });
        $("#lnk-contact-us").on("click touchend", () => {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.addClass("display-none");

            indexManager.elements.menuBar.removeClass("has-submenu");
            indexManager.elements.mainFrame.attr("src", indexManager.urls.contactUs);
        });
        $("#menu-back").on("click touchend", () => {
            var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
            var historyIndex = history ? parseInt(history, 0) : 0;

            if (historyIndex == 0) {
                return;
            }

            var key = indexManager.elements.currentURLCookieName + "_" + historyIndex;
            historyIndex--;

            siteInfrastructure.localStorage.remove(key);
            siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);

            indexManager.restoreUrl();
        });

        $("#lnk-my-setting").on("click touchend", () => {
        });
        $("#lnk-password-recovery").on("click touchend", () => {
        });
        $("#lnk-change-password").on("click touchend", () => {
            indexManager.topWindow.popupManager.init($, top);
            indexManager.topWindow.popupManager.open({
                title: indexManager.resources.changePassword,
                url: indexManager.urls.changePassword,
                top: 128,
                width: 490,
                height: 500
            });
        });
        $("#lnk-security-code").on("click touchend", () => {
            indexManager.topWindow.popupManager.init($, top);
            indexManager.topWindow.popupManager.open({
                title: indexManager.resources.changeSecurityCode,
                url: indexManager.urls.changeSecurityCode,
                top: 128,
                width: 490,
                height: 420
            });
        });
        $("#lnk-logout").on("click touchend", () => {
            siteInfrastructure.logout();
        });

        $("#sub-menu-OLDREPORT").parent().off("click touchend").on("click touchend", () => {
            $.blockUI();
            indexManager.currentState.isOpenPopup = true;
            var $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
            $.unblockUI();
            indexManager.currentState.isOpenPopup = false;
            indexManager.viewOldReport();
        });

        $("#main").on("load", () => {
            top.$.unblockUI();
            var currentUrl = top.main.location.href;
            var activeMainMenu = $("#main-menu-items .menu-item.active");
            var activeMainMenuId = activeMainMenu.length ? activeMainMenu.attr("id") : null;

            var activeSubmenu = activeMainMenuId != null ?
                $(".sub-menu[data-main-menu='" + activeMainMenuId + "'] .sub-menu-item.active > a") : null;

            indexManager.addHistory({
                mainMenu: activeMainMenuId,
                subMenu: activeSubmenu.length ? activeSubmenu.attr("id") : null,
                url: currentUrl
            });
        });

        setTimeout(indexManager.reCheckin, 30000);
        if (indexManager.elements.isNotHeadLine === 1) {
            setTimeout(indexManager.GetTotalOnlineMember, indexManager.elements.intervalToGetTotalOnlineMember);
        }
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

    GetTotalOnlineMember: function () {
        $.ajax({
            type: "GET",
            url: indexManager.urls.getTotalOnlineMemberUrl,
            success: function (result) {
                $("#numberOnlineMember").html(result);
            },
            complete: function () {
                setTimeout(indexManager.GetTotalOnlineMember, indexManager.elements.intervalToGetTotalOnlineMember);
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
        window.open(indexManager.urls.onlineList, "OnlineList", "height=600,width=800");
    },
    viewOldReport: () => {
        window.open(indexManager.urls.oldReportsPath, "OldReport", "height=750,width=960");
        $.unblockUI();
    },
    mainMenuClick: (id) => {
        if (id == "menu-back") {
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        var $this = $("#" + id);

        $this.addClass("active");

        indexManager.currentState.mainMenuId = id;
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        var subMenu = $(".sub-menu[data-main-menu='" + id + "']");
        if (subMenu && subMenu.length) {
            subMenu.removeClass("display-none");
            indexManager.elements.menuBar.addClass("has-submenu");
            subMenu.find(".sub-menu-item >.redirect-item:first")[0].click();
        } else {
            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0].childNodes[1]).href;
        }
    },
    subMenuClick: (id) => {
        indexManager.elements.subMenuItems.removeClass("active");
        var $this = $("#" + id);

        $this.parent().addClass("active");
        if (!indexManager.currentState.isOpenPopup) {
            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0]).href;
        }
    },
    addHistory: (newEvent) => {
        var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        var historyIndex = history ? parseInt(history, 0) : 0;
        historyIndex++;
        if (historyIndex == 21) {
            siteInfrastructure.localStorage.shiftStorage(indexManager.elements.currentURLCookieName, newEvent, historyIndex);
            return;
        }

        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName + "_" + historyIndex, newEvent);
    },
    restoreUrl: () => {
        var history = siteInfrastructure.localStorage.get(indexManager.elements.currentURLCookieName);
        var historyIndex = history ? parseInt(history, 0) : 0;

        if (historyIndex == 0) {
            indexManager.elements.mainMenu.find("a")[1].click();
            return;
        }

        var key = indexManager.elements.currentURLCookieName + "_" + historyIndex;
        var currentPage = siteInfrastructure.localStorage.get(key);
        historyIndex--;

        siteInfrastructure.localStorage.remove(key);
        siteInfrastructure.localStorage.set(indexManager.elements.currentURLCookieName, historyIndex);

        if (currentPage.mainMenu == null) {
            indexManager.elements.mainMenu.find("a")[1].click();
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        $("#" + currentPage.mainMenu).addClass("active");

        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.elements.subMenuItems.removeClass("active");

        var subMenu = $(".sub-menu[data-main-menu='" + currentPage.mainMenu + "']");
        if (subMenu && subMenu.length) {
            subMenu.removeClass("display-none");
            indexManager.elements.menuBar.addClass("has-submenu");
        }

        if (currentPage.subMenu != null) {
            $("#" + currentPage.subMenu).parent().addClass("active");
        }

        $("#main").attr("src", currentPage.url);
    },
    reCheckin: function () {
        $.ajax({
            type: "POST",
            url: indexManager.urls.reCheckin,
            data: { onid: indexManager.elements.onlineUserId },
            success: (result) => {
                try {
                    var message = result.Message;
                    if (!result.IsSuccess) {
                        if (message) {
                            alert(message);
                        } else {
                            alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                        }

                        siteInfrastructure.logout();
                    }
                }
                catch (e) {
                    if (typeof (result) === "string"
                        && result.indexOf("Oops... you are here because of a system error") > 0) {
                        siteInfrastructure.logout();
                        alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                    }
                }
            },
            error: (xhr) => {
                if (xhr.status === 500) {
                    siteInfrastructure.logout();
                    alert((<HTMLInputElement>document.getElementById("errorMessage")).value);
                } else if (xhr.status === 401 || xhr.status === 403) {
                    siteInfrastructure.logout();
                }
            },
            complete: () => {
                setTimeout(indexManager.reCheckin, 30000);
            }
        });
    },
    activeMainMenu: (id) => {
        indexManager.elements.mainMenu.removeClass("active");
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.currentState.mainMenuId = id;
        $("#" + id).addClass("active");
    },
    activeSubMenu: (id, mainMenuId) => {
        indexManager.elements.subMenu.addClass("display-none");
        indexManager.elements.menuBar.addClass("has-submenu");
        $(".sub-menu[data-main-menu='" + mainMenuId + "']").removeClass("display-none");

        $("#" + id).parent().addClass("active");
    }
};

$(function () {
    indexManager.init(window.top);
});