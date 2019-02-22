namespace CN88.Auth.Services
{
    using System.Text.RegularExpressions;
    using AppResources;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Sunplus.Agent.Utilities;

    public interface SecurityCodeService
    {
        SCResult ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        SCResult ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        SCResult ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        string GenerateSecurityToken();

        string GenerateSalt(int custId);

        SCResult VerifySecurityCode(VerifySecurityCodeParam param);
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

        public SCResult ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var validateResult = Validate(param.NewSecurityCode, param);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            return UpdateSecurityCode(new UpdateMainAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                UserId = param.UserId,
                HashedSecurityCode = HashSecurityCode(param.CustId, param.NewSecurityCode, param.IsResetMember),
                IP = param.IP,
                Language = param.Language
            });
        }

        public SCResult ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
        {
            var validateResult = Validate(param.NewSecurityCode, param);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            return UpdateSecurityCode(new UpdateSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            });
        }

        public SCResult ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
        {
            var validateResult = Validate(param.NewSecurityCode);

            if (!validateResult.IsOk)
            {
                return validateResult;
            }

            return UpdateSecurityCode(new ResetSubAccountSecurityCodeCriteria
            {
                CustId = param.CustId,
                SubaccId = param.SubAccId,
                UserId = param.UserId,
                HashedSecurityCode = HashSecurityCode(param.SubAccId, param.NewSecurityCode),
                IP = param.IP,
                Language = param.Language
            });
        }

        public string GenerateSecurityToken()
            => encryptor.RandomString(6);

        public string GenerateSalt(int custId)
        {
            string plainText = custId.ToString();

            return encryptor.ComputeHash(MD5, plainText, SECURITYCODEPADDING);
        }

        public SCResult VerifySecurityCode(VerifySecurityCodeParam param)
        {
            if (string.IsNullOrEmpty(param.SecurityCode))
            {
                return SCResult.CreateErrorResult(AuthResource.InformSecCode);
            }

            if (string.IsNullOrEmpty(param.SecurityCode) || string.IsNullOrEmpty(param.Salt))
            {
                return SCResult.CreateErrorResult(AuthResource.InputWrongSecCodeError);
            }

            // Database store Security Code hashed with CustID/SubAccID Salt
            // But Security code send from client browser has been hashed with CustID/SubAccID Salt by SHA256 Algorithm
            // So we have to hased security code from database with CustID/SubAccID Salt by SHA256 first, then compare
            // with hased security code which was sent from client browser.
            var actualSecurityCode = GetSecurityCode(param.CustId, param.SubAccId);
            var hashedActualSecurityCodeWithSalt = encryptor.ComputeHash(SHA256, actualSecurityCode, param.Salt);
            var isCorrectSecurityCode = string.Equals(param.SecurityCode, hashedActualSecurityCodeWithSalt);

            //// update counter after verify
            return UpdateSecurityCodeCounter(param.CustId, isCorrectSecurityCode, param.Language, param.SubAccId, param.IP);
        }

        private SCResult Validate(
            string securityCode,
            ChangeSecurityCodeParam param = null)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return SCResult.CreateErrorResult(AuthResource.SecCodeEmptyError);
            }

            if (!IsMatchedRules(securityCode))
            {
                return SCResult.CreateErrorResult(AuthResource.SecCodeWrongRuleError);
            }

            if (param != null && param.MustVerifyCurrentSecurityCode)
            {
                if (string.IsNullOrWhiteSpace(param.CurrentSecurityCode))
                {
                    return SCResult.CreateErrorResult(AuthResource.OldSecCodeEmptyError);
                }

                var verifyResult = VerifySecurityCode(new VerifySecurityCodeParam
                {
                    CustId = param.CustId,
                    SecurityCode = param.CurrentSecurityCode,
                    IP = param.IP,
                    Language = param.Language,
                    Salt = param.Salt,
                    SubAccId = param.SubAccId
                });

                if (verifyResult.IsNotOk)
                {
                    return verifyResult;
                }
            }

            return new SCResult();
        }

        private static bool IsMatchedRules(string securityCode)
        {
            int securityCodeLength = securityCode == null ? 0 : securityCode.Trim().Length;

            if (securityCodeLength != AllowSecurityCodeLength)
            {
                return false;
            }

            Regex regex = new Regex(SecurityCodeRule);
            //// check digits and all of them are not the same
            bool isMatched = regex.IsMatch(securityCode);

            if (isMatched)
            {
                //// check 'Forward Run' and 'Backward Run'
                bool isForwardRun = true, isBackwardRun = true;
                int forward = 0, backward = 0;
                forward = backward = int.Parse(securityCode[0].ToString());

                for (int i = 1; i < securityCodeLength; i++)
                {
                    int current = int.Parse(securityCode[i].ToString());

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
            //// because the Member site doesn't want to edit his code.
            //// so that we must use the CfsEnCode method.
            if (isResetMember)
            {
                return encryptor.CfsEnCode(securityCode);
            }

            var salt = GenerateSalt(custId);

            return encryptor.ComputeHash(SHA1, securityCode, salt);
        }

        private string GetSecurityCode(int custId, int subAccountId)
        {
            ICriteria criteria;

            if (subAccountId == 0)
            {
                criteria = new GetMainAccountSecurityCodeCriteria
                {
                    CustId = custId
                };
            }
            else
            {
                criteria = new GetSubAccountSecurityCodeCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId
                };
            }

            return dynamicRepository.Get<string>(criteria);
        }

        private SCResult UpdateSecurityCodeCounter(
            int custId,
            bool isCorrectSecurityCode,
            string language,
            int subAccountId,
            string ip)
        {
            SCNonQueryCommand command;

            if (subAccountId == 0)
            {
                command = new UpdateMainAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    CheckedSecurityCode = isCorrectSecurityCode,
                    IP = ip,
                    Language = language
                };
            }
            else
            {
                command = new UpdateSubAccountSecurityCodeCounterCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId,
                    CheckedSecurityCode = isCorrectSecurityCode,
                    IP = ip,
                    Language = language
                };
            }

            var result = UpdateSecurityCode(command);

            if (result.Code == 200)
            {
                // Account is closed.
                return SCResult.CreateErrorResult(result.Message, 2);
            }

            return result.IsFailed
                ? SCResult.CreateErrorResult(AuthResource.InputWrongSecCodeError, 9009)
                : result;
        }

        private SCResult UpdateSecurityCode(SCNonQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return new SCResult(
                command.ErrCode,
                command.ErrCode == 0 ? AuthResource.UpdatedSuccessfully : command.ErrMsg);
        }
    }
}
