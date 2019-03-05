namespace CN88.Auth.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AppResources;
    using Fanex.Cryptography.Aes;
    using Models;
    using Sunplus.Agent.Utilities;

    public interface PasswordValidator
    {
        string Encode(string password);

        string Decrypt(string password, string encryptKey);

        SecurityResult ValidateEmpty(string currentPassword, string newPassword);

        SecurityResult ValidateEmpty(string newPassword);

        SecurityResult IsMatchSMAPasswordRule(string password, IEnumerable<string> invalidPasswords, string oldPassword = null);

        SecurityResult IsMatchMemberPasswordRule(string password, IEnumerable<string> invalidPasswords);
    }

    public class PasswordValidatorImpl : PasswordValidator
    {
        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{8,100}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{8,100}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{8,100}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$");
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        private readonly Encryptor encryptor;

        public PasswordValidatorImpl(Encryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        public string Encode(string password)
            => encryptor.CfsEnCode(password);

        public string Decrypt(string password, string encryptKey)
            => AesExtension.Decrypt(password, encryptKey);

        public SecurityResult ValidateEmpty(string currentPassword, string newPassword)
            => string.IsNullOrWhiteSpace(currentPassword)
            ? SecurityResult.CreateErrorResult(AuthResource.RequiredInputOldPwdError)
            : ValidateEmpty(newPassword);

        public SecurityResult ValidateEmpty(string newPassword)
            => string.IsNullOrWhiteSpace(newPassword)
            ? SecurityResult.CreateErrorResult(AuthResource.RequiredPasswordError)
            : new SecurityResult();

        public SecurityResult IsMatchSMAPasswordRule(string password, IEnumerable<string> invalidPasswords, string oldPassword = null)
        {
            if (oldPassword != null && oldPassword == password)
            {
                return SecurityResult.CreateErrorResult(AuthResource.NewPwdSameOldOneError);
            }

            if (IsEqualsPasswordHint(password))
            {
                return SecurityResult.CreateErrorResult(AuthResource.PasswordWrongRuleError);
            }

            if (IsContainsInvalidChars(password, invalidPasswords))
            {
                return SecurityResult.CreateErrorResult(AuthResource.NewPwdContainsUserNameOrNickNameError);
            }

            var rule1 = PasswordRule1.Match(password).Success ? 1 : 0;
            var rule2 = PasswordRule2.Match(password).Success ? 1 : 0;
            var rule3 = PasswordRule3.Match(password).Success ? 1 : 0;
            var rule4 = PasswordRule4.Match(password).Success ? 1 : 0;

            return (rule1 + rule2 + rule3 + rule4 >= 3) && NoWhiteSpace.Match(password).Success
                ? new SecurityResult()
                : SecurityResult.CreateErrorResult(AuthResource.PasswordWrongRuleError);
        }

        public SecurityResult IsMatchMemberPasswordRule(string password, IEnumerable<string> invalidPasswords)
        {
            if (IsEqualsPasswordHint(password))
            {
                return SecurityResult.CreateErrorResult(AuthResource.PasswordWrongRuleError);
            }

            if (IsContainsInvalidChars(password, invalidPasswords))
            {
                return SecurityResult.CreateErrorResult(AuthResource.NewPwdContainsUserNameOrNickNameError);
            }

            return MemberPasswordRule.Match(password).Success && NoWhiteSpace.Match(password).Success
                ? new SecurityResult()
                : SecurityResult.CreateErrorResult(AuthResource.PasswordWrongRuleError);
        }

        private static bool IsEqualsPasswordHint(string password)
            => password.Equals("59D7!4$h", StringComparison.OrdinalIgnoreCase) ||
            password.Equals("493abcDE", StringComparison.OrdinalIgnoreCase);

        private static bool IsContainsInvalidChars(string password, IEnumerable<string> invalidPasswords)
            => invalidPasswords.Any(item => password.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);
    }
}
