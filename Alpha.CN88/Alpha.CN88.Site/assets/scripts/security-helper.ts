interface Window {
    hasPopupOpen(): boolean;
    securityCodePopup: any;
    setSecurityCodePopupTitle(securityCodeTitle): void;
    revertSecurityCodePopupTitle(): void;
    ageWnd: any;
}

interface String {
    hc(): void;
    ec(text: string): void;
}

declare var Sha1: any;
declare var Sha256: any;

var securityHelper = {
    validatePassword: function (container, isChangeMemberPassword = false) {
        var $container = $(container)
        var password = $container.val();
        var validPassword = securityHelper.isValidPassword(password, isChangeMemberPassword);
        validPassword ? securityHelper.showSuccessfullyIcon($container) : securityHelper.showFailIcon($container);
    },
    isValidPassword: function (password, isChangeMemberPassword = false) {
        var whiteSpace = /^\S*$/;
        var whiteSpaceCase = password.search(whiteSpace) != -1;

        if (isChangeMemberPassword) {
            var memberRegex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$/;

            return (password.search(memberRegex) != -1) && whiteSpaceCase;
        }
        else {
            var regex1 = /^(?=.{8,100}$).*\d/i;
            var regex2 = /^(?=.{8,100}$).*[a-z]/;
            var regex3 = /^(?=.{8,100}$).*[A-Z]/;
            var regex4 = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

            var case1 = password.search(regex1) != -1 ? 1 : 0;
            var case2 = password.search(regex2) != -1 ? 1 : 0;
            var case3 = password.search(regex3) != -1 ? 1 : 0;
            var case4 = password.search(regex4) != -1 ? 1 : 0;

            return (case1 + case2 + case3 + case4 >= 3) && whiteSpaceCase;
        }
    },
    signOut: function () {
        window.location.href = $("#mainRootPath").val() + "Auth/Logout";
    },
    redirectToHomePage: function (timeout) {
        setTimeout(securityHelper.gotoHomePage, timeout);
    },
    gotoHomePage: function () {
        window.location.href = $("#mainRootPath").val();
    },
    showSuccessfullyIcon: function ($container) {
        $container.removeClass('error');
        $container.addClass('valid');
    },
    showFailIcon: function ($container) {
        $container.removeClass('valid');
        $container.addClass('error');
    },
    showHint: function () {
        $('input[type=password]').each(function () {
            $(this).removeAttr("type").prop('type', 'text');
        })
    },
    hideHint: function () {
        $('input[type=text]').each(function () {
            $(this).removeAttr("type").prop('type', 'password');
        })
    },
};

String.prototype.ec = function (key) {
    var pt = this;
    var s = new Array();
    for (var i = 0; i < 256; i++) {
        s[i] = i;
    }
    var j = 0;
    var x;
    for (i = 0; i < 256; i++) {
        j = (j + s[i] + key.charCodeAt(i % key.length)) % 256;
        x = s[i];
        s[i] = s[j];
        s[j] = x;
    }
    i = 0;
    j = 0;
    var ct = '';
    for (var y = 0; y < pt.length; y++) {
        i = (i + 1) % 256;
        j = (j + s[i]) % 256;
        x = s[i];
        s[i] = s[j];
        s[j] = x;
        ct += String.fromCharCode(pt.charCodeAt(y) ^ s[(s[i] + s[j]) % 256]);
    }
    return ct;
};

String.prototype.hc = function () {
    var b16_digits = '0123456789abcdef';
    var b16_map = new Array();
    for (var i = 0; i < 256; i++) {
        b16_map[i] = b16_digits.charAt(i >> 4) + b16_digits.charAt(i & 15);
    }
    var result = new Array();
    for (var i = 0; i < this.length; i++) {
        result[i] = b16_map[this.charCodeAt(i)];
    }
    return result.join('');
};