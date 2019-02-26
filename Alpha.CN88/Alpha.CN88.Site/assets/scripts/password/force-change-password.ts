"use strict";

const forceChangePassword = {
    elements: {
        currentPassword: null,
        newPassword: null,
        confirmPassword: null,
        hashKey: null,
        messsageContainer: null
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

        $("#buttonBack").click(siteInfrastructure.logout);

        forceChangePassword.elements.currentPassword.focus();

        forceChangePassword.elements.newPassword.keyup(function () {
            passwordHelper.validatePassword(this);
        });

        forceChangePassword.elements.confirmPassword.keyup(function () {
            passwordHelper.validatePassword(this);
        });
    },
    cacheElements: function () {
        forceChangePassword.elements.currentPassword = $("#currentPassword");
        forceChangePassword.elements.newPassword = $("#newPassword");
        forceChangePassword.elements.confirmPassword = $("#confirmPassword");
        forceChangePassword.elements.hashKey = $("#HashKey").val();
        forceChangePassword.elements.messsageContainer = $("#message-container");
    },
    validatePasswords: function () {
        const $currentPassword = forceChangePassword.elements.currentPassword.removeClass("error");
        const $newPassword = forceChangePassword.elements.newPassword.removeClass("error");
        const $confirmPassword = forceChangePassword.elements.confirmPassword.removeClass("error");

        const currentPasswordValue = $currentPassword.val();
        const newPasswordValue = $newPassword.val();
        const confirmPasswordValue = $confirmPassword.val();

        if (currentPasswordValue.length === 0) {
            siteInfrastructure.showError(
                forceChangePassword.elements.messsageContainer,
                passwordResources.pleaseEnterOldPassword);
            $currentPassword.focus();

            return false;
        }

        if (newPasswordValue.length === 0) {
            siteInfrastructure.showError(
                forceChangePassword.elements.messsageContainer,
                passwordResources.passwordCannotEmpty);
            $newPassword.focus();

            return false;
        }

        if (!passwordHelper.isValidPassword(newPasswordValue)) {
            siteInfrastructure.showError(
                forceChangePassword.elements.messsageContainer,
                passwordResources.passwordDoesNotMeetMinimunRequirement);
            $newPassword.focus();

            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            siteInfrastructure.showError(
                forceChangePassword.elements.messsageContainer,
                passwordResources.confirmPasswordNotCorret);
            $confirmPassword.focus();

            return false;
        }

        if (newPasswordValue === currentPasswordValue) {
            siteInfrastructure.showError(
                forceChangePassword.elements.messsageContainer,
                passwordResources.newPassMustbeDiffOldPass);
            $newPassword.focus();

            return false;
        }

        return true;
    }
}

$(function () {
    forceChangePassword.initialize();
});