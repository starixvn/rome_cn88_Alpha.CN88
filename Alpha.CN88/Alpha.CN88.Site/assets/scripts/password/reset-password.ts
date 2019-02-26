"use strict";

const resetPassword = {
    elements: {
        newPassword: null,
        confirmPassword: null,
        hashKey: null,
        messsageContainer: null
    },
    initialize: function () {
        resetPassword.cacheElements();

        $("#formPassword").submit(function (e) {
            $.blockUI();
            if (!resetPassword.validatePasswords()) {
                $.unblockUI();
                return false;
            }

            const hashKey = resetPassword.elements.hashKey;
            const newPWd = resetPassword.elements.newPassword.val().ec(hashKey).hc();

            resetPassword.elements.newPassword.val(newPWd);
            resetPassword.elements.confirmPassword.val(newPWd);

            return true;
        });

        //$("#buttonBack").click(siteInfrastructure.logout);
        resetPassword.elements.newPassword.focus();
    },
    cacheElements: function () {
        resetPassword.elements.newPassword = $("#newPassword");
        resetPassword.elements.confirmPassword = $("#confirmPassword");
        resetPassword.elements.hashKey = $("#HashKey").val();
        resetPassword.elements.messsageContainer = $("#message-container");
    },
    validatePasswords: function () {
        const $newPassword = resetPassword.elements.newPassword.removeClass("error");
        const $confirmPassword = resetPassword.elements.confirmPassword.removeClass("error");

        const newPasswordValue = $newPassword.val();
        const confirmPasswordValue = $confirmPassword.val();

        if (newPasswordValue.length === 0) {
            siteInfrastructure.showError(
                resetPassword.elements.messsageContainer,
                passwordResources.passwordCannotEmpty);
            $newPassword.focus();

            return false;
        }

        if (!passwordHelper.isValidPassword(newPasswordValue)) {
            siteInfrastructure.showError(
                resetPassword.elements.messsageContainer,
                passwordResources.passwordDoesNotMeetMinimunRequirement);
            $newPassword.focus();

            return false;
        }

        if (newPasswordValue !== confirmPasswordValue) {
            siteInfrastructure.showError(
                resetPassword.elements.messsageContainer,
                passwordResources.confirmPasswordNotCorret);
            $confirmPassword.focus();

            return false;
        }

        return true;
    }
}

$(function () {
    resetPassword.initialize();
});