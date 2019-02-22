namespace CN88.Auth.Services
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AppResources;
    using Criteria.SecurityCode;
    using Fanex.Data.Repository;
    using Models;
    using Params.SecurityCode;
    using Sunplus.Agent.Utilities;

    public interface SecurityCodeService
    {
        Task<SecurityResult> ChangeMainAccountSecurityCodeAsync(ChangeSecurityCodeParam param);

        Task<SecurityResult> ChangeSubAccountSecurityCodeAsync(ChangeSecurityCodeParam param);

        Task<SecurityResult> ResetSubAccountSecurityCodeAsync(ResetSecurityCodeParam param);

        string GenerateSecurityToken();

        string GenerateSalt(int custId);

        Task<SecurityResult> VerifySecurityCodeAsync(VerifySecurityCodeParam param);
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

        public async Task<SecurityResult> ChangeMainAccountSecurityCodeAsync(ChangeSecurityCodeParam param)
        {
            var validateResult = await ValidateAsync(param.NewSecurityCode, param);

            if (validateResult.IsFail)
            {
                return validateResult;
            }

            return await UpdateSecurityCodeAsync(new UpdateMainAccountSecurityCodeCriteria(
                param.CustId,
                param.SubAccId,
                HashSecurityCode(param.CustId, param.NewSecurityCode, param.IsResetMember),
                param.UserId,
                param.IP,
                param.LanguageKey));
        }

        public async Task<SecurityResult> ChangeSubAccountSecurityCodeAsync(ChangeSecurityCodeParam param)
        {
            var validateResult = await ValidateAsync(param.NewSecurityCode, param);

            if (validateResult.IsFail)
            {
                return validateResult;
            }

            return await UpdateSecurityCodeAsync(new UpdateSubAccountSecurityCodeCriteria(
                param.CustId,
                param.SubAccId,
                HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                param.IP,
                param.LanguageKey));
        }

        public async Task<SecurityResult> ResetSubAccountSecurityCodeAsync(ResetSecurityCodeParam param)
        {
            var validateResult = await ValidateAsync(param.NewSecurityCode);

            if (validateResult.IsFail)
            {
                return validateResult;
            }

            return await UpdateSecurityCodeAsync(new ResetSubAccountSecurityCodeCriteria(
                param.CustId,
                param.SubAccId,
                HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                param.UserId,
                param.IP,
                param.LanguageKey));
        }

        public string GenerateSecurityToken()
            => encryptor.RandomString(6);

        public string GenerateSalt(int custId)
        {
            string plainText = custId.ToString();

            return encryptor.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        public async Task<SecurityResult> VerifySecurityCodeAsync(VerifySecurityCodeParam param)
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
            var actualSecurityCode = await GetSecurityCodeAsync(param.CustId, param.SubAccId);
            var hashedActualSecurityCodeWithSalt = encryptor.ComputeHash(SHA256, actualSecurityCode, param.Salt);
            var isCorrectSecurityCode = string.Equals(param.SecurityCode, hashedActualSecurityCodeWithSalt);

            // Update counter after verify.
            return await UpdateSecurityCodeCounterAsync(param.CustId, isCorrectSecurityCode, param.LanguageKey, param.SubAccId, param.IP);
        }

        private async Task<SecurityResult> ValidateAsync(
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

            if (param != null && param.MustVerifyCurrentSecurityCode)
            {
                if (string.IsNullOrWhiteSpace(param.CurrentSecurityCode))
                {
                    return SecurityResult.CreateErrorResult(AuthResource.OldSecCodeEmptyError);
                }

                var verifyResult = await VerifySecurityCodeAsync(new VerifySecurityCodeParam(
                    param.CustId,
                    param.SubAccId,
                    param.CurrentSecurityCode,
                    param.Salt,
                    param.IP,
                    param.LanguageKey));

                if (verifyResult.IsFail)
                {
                    return verifyResult;
                }
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

        private string HashSecurityCode(int custId, string securityCode, bool isResetMember = false)
        {
            // Because the Member site doesn't want to edit his code.
            // So that we must use the CfsEnCode method.
            if (isResetMember)
            {
                return encryptor.CfsEnCode(securityCode);
            }

            var salt = GenerateSalt(custId);

            return encryptor.ComputeHash(SHA1, securityCode, salt);
        }

        private async Task<string> GetSecurityCodeAsync(int custId, int subAccountId)
        {
            ICriteria criteria = subAccountId == 0
                ? new GetMainAccountSecurityCodeCriteria(custId)
                : new GetSubAccountSecurityCodeCriteria(custId, subAccountId);

            return await dynamicRepository.GetAsync<string>(criteria);
        }

        private async Task<SecurityResult> UpdateSecurityCodeCounterAsync(
            int custId,
            bool isCorrectSecurityCode,
            string language,
            int subAccountId,
            string ip)
        {
            SecurityCodeNonQueryCommand command = subAccountId == 0
                ? new UpdateMainAccountSecurityCodeCounterCriteria(custId, isCorrectSecurityCode, ip, language)
                : new UpdateSubAccountSecurityCodeCounterCriteria(custId, subAccountId, isCorrectSecurityCode, ip, language);

            var result = await UpdateSecurityCodeAsync(command);

            if (result.Code == 200)
            {
                // Account is closed.
                return SecurityResult.CreateErrorResult(result.Message, 2);
            }

            return result.IsFail
                ? SecurityResult.CreateErrorResult(AuthResource.InputWrongSecCodeError, 9009)
                : result;
        }

        private async Task<SecurityResult> UpdateSecurityCodeAsync(SecurityCodeNonQueryCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return SecurityResult.CreateErrorResult(
                command.ErrCode == 0 ? AuthResource.UpdatedSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}
