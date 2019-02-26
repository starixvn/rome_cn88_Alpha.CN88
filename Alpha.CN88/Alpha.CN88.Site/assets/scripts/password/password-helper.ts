"use strict";
const passwordResources = {
    pleaseEnterOldPassword: (<HTMLInputElement>document.getElementById('please-enter-oldPassword')).value,
    passwordCannotEmpty: (<HTMLInputElement>document.getElementById('password-cannot-empty')).value,
    confirmPasswordNotCorret: (<HTMLInputElement>document.getElementById('not-correct-confirm-password')).value,
    changePasswordSuccessfully: (<HTMLInputElement>document.getElementById('password-change-successfully')).value,
    newPassMustbeDiffOldPass: (<HTMLInputElement>document.getElementById('newPassword-must-be-diff-old')).value,
    passwordDoesNotMeetMinimunRequirement: (<HTMLInputElement>document.getElementById('password-does-not-min-requirement')).value
};

const passwordHelper = {
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
    }
};