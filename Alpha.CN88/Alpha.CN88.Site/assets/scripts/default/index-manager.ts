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
        oldReportsPath: null,
        wagerMonitoringConsolePath: null
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
        header: null,
        content: null,
        listMenu: null,
        hamburgerContainer: null,
        hamburgerIcon: null,
        isDevice: false,
        pageTitle: null,
        announcementPath: null
    },
    currentState: {
        mainMenuId: null,
        isOpenPopup: false,
        isMainMenuClick: false,
        isScrolling: false,
        isClearActiveMenu: false,
        wagerMonitoringWindow: null
    },
    resources: {
        changePassword: null,
        changeSecurityCode: null,
        mobileHeaderName: "mobileHeaderName"
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
        indexManager.urls.wagerMonitoringConsolePath = $("#WagerMonitoringConsolePath").val();

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
        indexManager.elements.header = $("#header");
        indexManager.elements.content = $("#content");
        indexManager.elements.listMenu = $("#main-menu-items");
        indexManager.elements.hamburgerContainer = $("#hamburger-container");
        indexManager.elements.hamburgerIcon = indexManager.elements.hamburgerContainer.children("#hamburgerIcon");
        indexManager.elements.isDevice = $("#IsDevice").val() == "1";
        indexManager.elements.pageTitle = $("#page-title");
        indexManager.elements.announcementPath = $("#AnnouncementPath").val();
    },
    bindEvents: function () {
        $(".changelanguage-link").on("click touchend", (event) => {
            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

            indexManager.changeLanguage($(event.target).data("value"));
        });
        $(".online-member").on("click touchend", function (event) {
            if (event.type == 'touchend') {
                $(this).off('click');
            }
            indexManager.viewOnlineList();
        });

        indexManager.elements.mainMenu.on("click touchend", function (event) {
            if (event.type == 'touchend') {
                $(this).off('click');
            }

            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

            $.blockUI();
            var $this = $(this);
            indexManager.mainMenuClick($this.attr("id"));
        });
        indexManager.elements.subMenuItems.on("click touchend", function (event) {
            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

            $.blockUI();
            var $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));

            event.stopPropagation();
        });

        $("#lnk-new-url").on("click touchend", () => {
            $("#menu-HELP").click();
        });

        $("#lnk-live-chat").on("click touchend", () => {
            window.open(indexManager.urls.liveChat, "LiveChat", "height=600,width=800");
        });
        $("#lnk-contact-us").on("click touchend", () => {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.slideUp("fast");

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
            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

            indexManager.topWindow.showCN88CancelButton = false;
            indexManager.currentState.isClearActiveMenu = true;
            indexManager.clearAllActiveMenu();
        });
        $("#lnk-password-recovery").on("click touchend", () => {
        });
        $("#lnk-change-password").on("click touchend", () => {
            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

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
            if (indexManager.currentState.isScrolling) {
                indexManager.currentState.isScrolling = false;
                event.stopPropagation();
                return false;
            }

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

        indexManager.elements.hamburgerContainer.on("click touchend", function (event) {

            if (event.type == 'touchend') {
                $(this).off('click');
            }

            var isCollapsed = indexManager.elements.listMenu.hasClass("collapsed");
            if (isCollapsed) {
                indexManager.elements.hamburgerIcon.addClass("active");
                indexManager.elements.listMenu.removeClass("collapsed");
            } else {
                indexManager.elements.listMenu.addClass("collapsed");
                indexManager.elements.hamburgerIcon.removeClass("active");
            }

            event.stopPropagation();
            return false;
        });

        $("#sub-menu-OLDREPORT")
            .parent()
            .off("click touchend")
            .on("click touchend", () => {
                $.blockUI();
                indexManager.currentState.isOpenPopup = true;
                var $this = $(this);
                indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
                $.unblockUI();
                indexManager.currentState.isOpenPopup = false;
                indexManager.viewOldReport();
            });

        $("#sub-menu-WAGERMONITORINGCONSOLE").parent().off("click touchend").on("click touchend", () => {
            $.blockUI();
            indexManager.currentState.isOpenPopup = true;
            var $this = $(this);
            indexManager.subMenuClick($this.find(">.redirect-item:first").attr("id"));
            $.unblockUI();
            indexManager.currentState.isOpenPopup = false;
            indexManager.viewWagerMonitoringConsole();
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
                subMenu: activeSubmenu && activeSubmenu.length ? activeSubmenu.attr("id") : null,
                url: currentUrl,
                isClearActiveMenu: indexManager.currentState.isClearActiveMenu
            });

            indexManager.currentState.isClearActiveMenu = false;
            indexManager.calculateMainFrameHeight();

            var iframeWindow = indexManager.elements.mainFrame[0].contentWindow || indexManager.elements.mainFrame[0].contentDocument;
            $(iframeWindow.document).on('click touchend', () => { indexManager.closeMenuBarOnMobile(); });
        });

        setTimeout(indexManager.reCheckin, 30000);
        if (indexManager.elements.isNotHeadLine === 1) {
            setTimeout(indexManager.GetTotalOnlineMember, indexManager.elements.intervalToGetTotalOnlineMember);
        }

        $(window).resize(() => {
            indexManager.calculateMainFrameHeight();
            if (indexManager.elements.isDevice) {
                indexManager.calculateUserMobileHeight();
            }
        });

        indexManager.initForMobile();
    },
    initForMobile: () => {
        if (indexManager.elements.isDevice) {
            $(document).on("touchmove", () => { indexManager.currentState.isScrolling = true; });

            $(document).on("touchend", () => { indexManager.currentState.isScrolling = false; });

            $(".username-mobile").on("click touchend", () => {
                indexManager.closeMenuBarOnMobile();
            });

            $(".menubar-mobile").on("click touchend", () => {
                indexManager.closeMenuBarOnMobile();
            });

            $(".icon-bell").on("click touchend", () => {
                indexManager.elements.mainFrame.attr("src", indexManager.elements.announcementPath);
            });      
     

            $('.username-mobile .dropdown').on('show.bs.dropdown', function () {
                indexManager.calculateUserMobileHeight();
            }).on('hide.bs.dropdown', function () {
                indexManager.calculateUserMobileHeight();
            });

            var iOS = !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
            if (iOS) {
                $('body').backtop();
            }
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
        if (indexManager.elements.isDevice) {
            window.open(indexManager.urls.onlineList, "_blank");
        }
        else {
            window.open(indexManager.urls.onlineList, "OnlineList", "height=600,width=800");
        }

    },
    viewOldReport: () => {
        if (indexManager.elements.isDevice) {
            window.open(indexManager.urls.oldReportsPath, "_blank");
        }
        else {
            window.open(indexManager.urls.oldReportsPath, "OldReport", "height=750,width=960");
        }
    },
    viewWagerMonitoringConsole: () => {
        $.unblockUI();
        if (indexManager.currentState.wagerMonitoringWindow == null || indexManager.currentState.wagerMonitoringWindow.closed) {
            indexManager.currentState.wagerMonitoringWindow = window.open(indexManager.urls.wagerMonitoringConsolePath, "WagerMonitoringConsole", "height=800,width=1280");
        } else {
            indexManager.currentState.wagerMonitoringWindow.focus();
        }
    },
    mainMenuClick: (id) => {
        indexManager.currentState.isMainMenuClick = true;

        if (id == "menu-back") {
            return;
        }

        var $this = $("#" + id);
        var subMenu = $(".sub-menu[data-main-menu='" + id + "']");
        var isCurrentActive = $this.hasClass("active");
        if (indexManager.elements.isDevice && isCurrentActive) {
            indexManager.elements.subMenu.slideUp("fast");
            indexManager.elements.menuBar.removeClass("has-submenu");
            $this.removeClass("active");
            indexManager.currentState.isMainMenuClick = false;
            top.$.unblockUI();
            return;
        }

        if (!isCurrentActive) {
            indexManager.elements.mainMenu.removeClass("active");
            indexManager.elements.subMenu.slideUp("fast");
            $this.addClass("active");
        }

        indexManager.currentState.mainMenuId = id;
        indexManager.elements.menuBar.removeClass("has-submenu");

        if (subMenu && subMenu.length) {
            indexManager.elements.menuBar.addClass("has-submenu");
            subMenu.slideDown("fast", () => {
                if (!indexManager.elements.isDevice) {
                    indexManager.calculateMenuBarHeight(subMenu);
                }
            });

            if (indexManager.elements.isDevice) {
                indexManager.currentState.isMainMenuClick = false;
                top.$.unblockUI();
            } else {
                subMenu.find(".sub-menu-item >.redirect-item:first")[0].click();
            }
        } else {

            indexManager.currentState.isMainMenuClick = false;

            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0].childNodes[1]).href;

            indexManager.closeMenuBarOnMobile();
            indexManager.elements.pageTitle.text($this.text().trim());
            if (!indexManager.elements.isDevice) {
                indexManager.calculateMenuBarHeight(subMenu);
            }
        }
    },
    subMenuClick: (id) => {
        indexManager.elements.subMenuItems.removeClass("active");
        var $this = $("#" + id);

        $this.parent().addClass("active");
        if (!indexManager.currentState.isOpenPopup) {
            (<HTMLIFrameElement>$("#main")[0]).src = (<HTMLLinkElement>$this[0]).href;
        }

        if (!indexManager.currentState.isMainMenuClick) {
            indexManager.closeMenuBarOnMobile();
        }

        indexManager.currentState.isMainMenuClick = false;

        if (indexManager.elements.isDevice) {
            var headerText = $this.text().trim();

            indexManager.setMobileHeaderName(headerText);
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
        if (currentPage.isClearActiveMenu) {
            top.showCN88CancelButton = !indexManager.isMySettingsPage(currentPage.url);
            indexManager.currentState.isClearActiveMenu = true;
            $("#main").attr("src", currentPage.url);
            return;
        }

        if (currentPage.mainMenu == null) {
            indexManager.elements.mainMenu.find("a")[1].click();
            return;
        }

        indexManager.elements.mainMenu.removeClass("active");
        $("#" + currentPage.mainMenu).addClass("active");

        indexManager.elements.subMenu.slideUp("fast");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.elements.subMenuItems.removeClass("active");

        var subMenu = $(".sub-menu[data-main-menu='" + currentPage.mainMenu + "']");
        if (subMenu && subMenu.length) {
            indexManager.elements.menuBar.addClass("has-submenu");
            subMenu.slideDown("fast", () => {
                if (!indexManager.elements.isDevice) {
                    indexManager.calculateMenuBarHeight(subMenu);
                }
            });
        }

        if (currentPage.subMenu != null) {
            $("#" + currentPage.subMenu).parent().addClass("active");
        }

        $("#main").attr("src", currentPage.url);

        if (indexManager.elements.isDevice) {
            indexManager.initMobileHeaderName();
        }
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
        indexManager.elements.subMenu.slideUp("fast");
        indexManager.elements.menuBar.removeClass("has-submenu");

        indexManager.currentState.mainMenuId = id;
        $("#" + id).addClass("active");
    },
    closePopupAndReloadMainIframe: () => {
        setTimeout(() => {
            indexManager.topWindow.popupManager.close();
            indexManager.elements.mainFrame[0].contentWindow.location.reload(true);
        }, 1000 * 3);
    },
    activeSubMenu: (id, mainMenuId) => {
        indexManager.elements.subMenu.slideUp("fast");
        indexManager.elements.menuBar.addClass("has-submenu");
        var subMenu = $(".sub-menu[data-main-menu='" + mainMenuId + "']");
        $("#" + id).parent().addClass("active");

        subMenu.slideDown("fast", () => {
            indexManager.calculateMenuBarHeight(subMenu);
        });
    },
    calculateMainFrameHeight: () => {
        var height = $(window).height()
            - indexManager.elements.header.height()
            - indexManager.elements.menuBar.height();
        indexManager.elements.content.height(height);
    },
    closeMenuBarOnMobile: () => {
        indexManager.elements.listMenu.addClass("collapsed");
        indexManager.elements.hamburgerIcon.removeClass("active");
    },
    calculateMenuBarHeight: (subMenu) => {
        var menuBarHeight = indexManager.elements.listMenu.height();
        menuBarHeight += subMenu.length ? subMenu.height() : 0;
        indexManager.elements.menuBar.height(menuBarHeight);
    },
    clearAllActiveMenu: () => {
        indexManager.elements.mainMenu.removeClass("active");
        indexManager.elements.subMenu.slideUp("fast");
        indexManager.elements.menuBar.removeClass("has-submenu");

        if (!indexManager.elements.isDevice) {
            indexManager.calculateMenuBarHeight([]);
        }
    },
    isMySettingsPage: (currentUrl) => {
        return currentUrl.toLowerCase().indexOf("cn88customersetting/viewcn88customersetting/mysetting") > -1;


    },
    setMobileHeaderName: (text) => {
        if (indexManager.elements.isDevice) {
            indexManager.elements.pageTitle.text(text);
            siteInfrastructure.localStorage.set(indexManager.resources.mobileHeaderName, text);
        }
    },
    initMobileHeaderName: () => {
        if (indexManager.elements.isDevice) {
            var text = siteInfrastructure.localStorage.get(indexManager.resources.mobileHeaderName);
            if (text) {
                indexManager.elements.pageTitle.text(text);
            }
        }
    },
    calculateUserMobileHeight: () => {
        var dropdownMenu = $(".username-mobile .dropdown-menu"),
            mainHeight = $(window).height() - $("#header").height(),
            dropdownMenuHeight = dropdownMenu.height();

        if (dropdownMenuHeight > mainHeight) {
            dropdownMenu.height(mainHeight);
        } else {
            dropdownMenu.css("height", "auto");
        }
    }
};

$(function () {
    indexManager.init(window.top);
});