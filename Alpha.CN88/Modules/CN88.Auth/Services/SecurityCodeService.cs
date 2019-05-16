namespace CN88.Auth.Services
{
    using System.Text.RegularExpressions;
    using AppResources;
    using Criteria.SecurityCode;
    using Fanex.Data.Repository;
    using Models;
    using Params.SecurityCode;
    using Sunplus.Agent.Utilities;

    public interface SecurityCodeService
    {
        SecurityResult ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        SecurityResult ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        SecurityResult ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        string GenerateSecurityToken();

        string GenerateSalt(int custId);

        SecurityResult VerifySecurityCode(VerifySecurityCodeParam param);
    }

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        private const string MD5 = "MD5";
        private const string SHA1 = "SHA1";
        private const string SHA256 = "SHA256";
        private const string SECURITYCODEPADDING = "1qaw3+{:P)";
        private const int AllowSecurityCodeLength = 6;
        private const string SecurityCodeRule = @"^([0-9])(?!\1+$)[0-9]+$";

        private readonly Encryptor encryptor;
        private readonly IDynamicRepository dynamicRepository;

        public SecurityCodeServiceImpl(
            Encryptor encryptor,
            IDynamicRepository dynamicRepository)
        {
            this.encryptor = encryptor;
            this.dynamicRepository = dynamicRepository;
        }

        public SecurityResult ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var result = Validate(param.NewSecurityCode, param);

            if (result.IsSuccess)
            {
                result = UpdateSecurityCode(new UpdateMainAccountSecurityCodeCriteria(
                    param.CustId,
                    param.SubAccId,
                    HashSecurityCode(param.CustId, param.NewSecurityCode),
                    param.UserId,
                    param.IP,
                    param.LanguageKey));
            }

            return result;
        }

        public SecurityResult ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var result = Validate(param.NewSecurityCode, param);

            if (result.IsSuccess)
            {
                result = UpdateSecurityCode(new UpdateSubAccountSecurityCodeCriteria(
                    param.CustId,
                    param.SubAccId,
                    HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                    param.IP,
                    param.LanguageKey));
            }

            return result;
        }

        public SecurityResult ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
        {
            var result = Validate(param.NewSecurityCode);

            if (result.IsSuccess)
            {
                result = UpdateSecurityCode(new ResetSubAccountSecurityCodeCriteria(
                    param.CustId,
                    param.SubAccId,
                    HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                    param.UserId,
                    param.IP,
                    param.LanguageKey));
            }

            return result;
        }

        public string GenerateSecurityToken() => encryptor.RandomString(6);

        public string GenerateSalt(int custId)
        {
            string plainText = custId.ToString();

            return encryptor.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        public SecurityResult VerifySecurityCode(VerifySecurityCodeParam param)
        {
            if (string.IsNullOrEmpty(param.SecurityCode))
            {
                return SecurityResult.CreateErrorResult(AuthResource.InformSecCode);
            }

            if (string.IsNullOrEmpty(param.SecurityCode) || string.IsNullOrEmpty(param.Salt))
            {
                return SecurityResult.CreateErrorResult(AuthResource.InputWrongSecCodeError);
            }

            // Database store Security Code hashed with CustID/SubAccID Salt
            // But Security code send from client browser has been hashed with CustID/SubAccID Salt by SHA256 Algorithm
            // So we have to hased security code from database with CustID/SubAccID Salt by SHA256 first, then compare
            // with hased security code which was sent from client browser.
            var actualSecurityCode = GetSecurityCode(param.CustId, param.SubAccId);
            var hashedActualSecurityCodeWithSalt = encryptor.ComputeHash(SHA256, actualSecurityCode, param.Salt);
            var isCorrectSecurityCode = string.Equals(param.SecurityCode, hashedActualSecurityCodeWithSalt);

            // Update counter after verify.
            return UpdateSecurityCodeCounter(param.CustId, isCorrectSecurityCode, param.LanguageKey, param.SubAccId, param.IP);
        }

        private SecurityResult Validate(
            string securityCode,
            ChangeSecurityCodeParam param = null)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return SecurityResult.CreateErrorResult(AuthResource.SecCodeEmptyError);
            }

            if (!IsMatchedRules(securityCode))
            {
                return SecurityResult.CreateErrorResult(AuthResource.SecCodeWrongRuleError);
            }

            if (param?.MustVerifyCurrentSecurityCode == true)
            {
                return VerifySecurityCode(new VerifySecurityCodeParam(
                    param.CustId,
                    param.SubAccId,
                    param.CurrentSecurityCode,
                    param.Salt,
                    param.IP,
                    param.LanguageKey));
            }

            return new SecurityResult();
        }

        private static bool IsMatchedRules(string securityCode)
        {
            var securityCodeLength = securityCode == null ? 0 : securityCode.Trim().Length;

            if (securityCodeLength != AllowSecurityCodeLength)
            {
                return false;
            }

            var regex = new Regex(SecurityCodeRule);
            // Check digits and all of them are not the same.
            bool isMatched = regex.IsMatch(securityCode);

            if (isMatched)
            {
                // Check 'Forward Run' and 'Backward Run'.
                var isForwardRun = true;
                var isBackwardRun = true;
                var forward = 0;
                var backward = 0;
                forward = backward = int.Parse(securityCode[0].ToString());

                for (int i = 1; i < securityCodeLength; i++)
                {
                    var current = int.Parse(securityCode[i].ToString());

                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                isMatched = !(isForwardRun || isBackwardRun);
            }

            return isMatched;
        }

        private string HashSecurityCode(int custId, string securityCode)
        {
            var salt = GenerateSalt(custId);

            return encryptor.ComputeHash(SHA1, securityCode, salt);
        }

        private string GetSecurityCode(int custId, int subAccountId)
        {
            ICriteria criteria = subAccountId == 0
                ? new GetMainAccountSecurityCodeCriteria(custId)
                : new GetSubAccountSecurityCodeCriteria(custId, subAccountId);

            return dynamicRepository.Get<string>(criteria);
        }

        private SecurityResult UpdateSecurityCodeCounter(
            int custId,
            bool isCorrectSecurityCode,
            string language,
            int subAccountId,
            string ip)
        {
            SecurityCodeNonQueryCommand command = subAccountId == 0
                ? new UpdateMainAccountSecurityCodeCounterCriteria(custId, isCorrectSecurityCode, ip, language)
                : new UpdateSubAccountSecurityCodeCounterCriteria(custId, subAccountId, isCorrectSecurityCode, ip, language);

            var result = UpdateSecurityCode(command);

            if (result.Code == 200)
            {
                // Account is closed.
                return SecurityResult.CreateErrorResult(result.Message, 2);
            }

            return result.IsFail
                ? SecurityResult.CreateErrorResult(AuthResource.InputWrongSecCodeError, 9009)
                : result;
        }

        private SecurityResult UpdateSecurityCode(SecurityCodeNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.SecCodeChangedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
