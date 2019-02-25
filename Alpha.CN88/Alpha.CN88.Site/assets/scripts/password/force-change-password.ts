"use strict";

var forceChangePassword = {
    elements: {
        currentPassword: null,
        newPassword: null,
        confirmPassword: null,
        hashKey: null
    },
    initialize: function () {
        forceChangePassword.cacheElements();

        $("#frmForceChangePassword").submit(function (e) {
            $.blockUI();
            if (!forceChangePassword.validatePasswords()) {
                $.unblockUI();
                return false;
            }

            const hashKey = forceChangePassword.elements.hashKey;
            const currentPwd = forceChangePassword.elements.currentPassword.val().ec(hashKey).hc();
            const newPWd = forceChangePassword.elements.newPassword.val().ec(hashKey).hc();

            forceChangePassword.elements.currentPassword.val(currentPwd);
            forceChangePassword.elements.newPassword.val(newPWd);
            forceChangePassword.elements.confirmPassword.val(newPWd);

            return true;
        });

        $("#btn-back").bind("click", function () {
            //securityHelper.signOut();
        });

        forceChangePassword.elements.currentPassword.focus();

        forceChangePassword.elements.newPassword.keyup(function () {
            securityHelper.validatePassword(this);
        });

        forceChangePassword.elements.confirmPassword.keyup(function () {
            securityHelper.validatePassword(this);
        });

        $("#showPasswordchk").click(function () {
            var isChecked = $(this).is(":checked");
            isChecked ? securityHelper.showHint() : securityHelper.hideHint();
        });
    },
    cacheElements: function () {
        forceChangePassword.elements.currentPassword = $("#txtOldPwd");
        forceChangePassword.elements.newPassword = $("#txtNewPwd");
        forceChangePassword.elements.confirmPassword = $("#txtConfirmPwd");
        forceChangePassword.elements.hashKey = $("#HashKey").val();
    },
    validatePasswords: function () {
        const $currentPassword = forceChangePassword.elements.currentPassword.removeClass("error");
        const $newPassword = forceChangePassword.elements.newPassword.removeClass("error");
        const $confirmPassword = forceChangePassword.elements.confirmPassword.removeClass("error");

        const currentPasswordValue = $currentPassword.val();
        const newPasswordValue = $newPassword.val();
        const confirmPasswordValue = $confirmPassword.val();

        if (currentPasswordValue.length === 0) {
            $currentPassword.addClass("error").focus();
            //passwordResources.pleaseEnterOldPassword
            return false;
        }

        if (newPasswordValue.length === 0) {
            $newPassword.addClass("error").focus();
            //passwordResources.rules
            return false;
        }

        if (!securityHelper.isValidPassword(newPasswordValue)) {
            $newPassword.addClass("error").focus();
            //passwordResources.rules
            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            $confirmPassword.addClass("error").focus();
            //passwordResources.confirmPasswordNotCorret
            return false;
        }

        if (newPasswordValue === currentPasswordValue) {
            $newPassword.addClass("error").focus();
            //passwordResources.newPassMustbeDiffOldPass
            return false;
        }

        return true;
    },
    resetOriginalInputStatus: function () {
        forceChangePassword.elements.currentPassword.removeClass("error");
        forceChangePassword.elements.newPassword.removeClass("error");
        forceChangePassword.elements.confirmPassword.removeClass("error");
    }
}

$(function () {
    forceChangePassword.initialize();
});