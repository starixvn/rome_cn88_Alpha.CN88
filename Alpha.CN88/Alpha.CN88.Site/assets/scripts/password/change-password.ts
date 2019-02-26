"use strict";

const changePassword = {
    elements: {
        currentPassword: null,
        newPassword: null,
        confirmPassword: null,
        hashKey: null,
        messsageContainer: null
    },
    initialize: function () {
        changePassword.cacheElements();

        $("#formPassword").submit(function (e) {
            $.blockUI();
            if (!changePassword.validatePasswords()) {
                $.unblockUI();
                return false;
            }

            const hashKey = changePassword.elements.hashKey;
            const currentPwd = changePassword.elements.currentPassword.val().ec(hashKey).hc();
            const newPWd = changePassword.elements.newPassword.val().ec(hashKey).hc();

            changePassword.elements.currentPassword.val(currentPwd);
            changePassword.elements.newPassword.val(newPWd);
            changePassword.elements.confirmPassword.val(newPWd);

            return true;
        });

        $("#buttonBack").click(function () {
            const forceChangePasswordPage = $("#mainRootPath").length > 0;
            if (forceChangePasswordPage) {
                siteInfrastructure.logout();
            } else {
                siteInfrastructure.closePopup();
            }
        });

        changePassword.elements.currentPassword.focus();
    },
    cacheElements: function () {
        changePassword.elements.currentPassword = $("#currentPassword");
        changePassword.elements.newPassword = $("#newPassword");
        changePassword.elements.confirmPassword = $("#confirmPassword");
        changePassword.elements.hashKey = $("#HashKey").val();
        changePassword.elements.messsageContainer = $("#message-container");
    },
    validatePasswords: function () {
        const $currentPassword = changePassword.elements.currentPassword.removeClass("error");
        const $newPassword = changePassword.elements.newPassword.removeClass("error");
        const $confirmPassword = changePassword.elements.confirmPassword.removeClass("error");

        const currentPasswordValue = $currentPassword.val();
        const newPasswordValue = $newPassword.val();
        const confirmPasswordValue = $confirmPassword.val();

        if (currentPasswordValue.length === 0) {
            siteInfrastructure.showError(
                changePassword.elements.messsageContainer,
                passwordResources.pleaseEnterOldPassword);
            $currentPassword.focus();

            return false;
        }

        if (newPasswordValue.length === 0) {
            siteInfrastructure.showError(
                changePassword.elements.messsageContainer,
                passwordResources.passwordCannotEmpty);
            $newPassword.focus();

            return false;
        }

        if (!passwordHelper.isValidPassword(newPasswordValue)) {
            siteInfrastructure.showError(
                changePassword.elements.messsageContainer,
                passwordResources.passwordDoesNotMeetMinimunRequirement);
            $newPassword.focus();

            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            siteInfrastructure.showError(
                changePassword.elements.messsageContainer,
                passwordResources.confirmPasswordNotCorret);
            $confirmPassword.focus();

            return false;
        }

        if (newPasswordValue === currentPasswordValue) {
            siteInfrastructure.showError(
                changePassword.elements.messsageContainer,
                passwordResources.newPassMustbeDiffOldPass);
            $newPassword.focus();

            return false;
        }

        return true;
    }
}

$(function () {
    changePassword.initialize();
});