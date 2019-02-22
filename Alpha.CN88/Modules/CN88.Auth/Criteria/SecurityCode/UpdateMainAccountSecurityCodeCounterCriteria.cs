namespace CN88.Auth.Criteria.SecurityCode
{
    public class UpdateMainAccountSecurityCodeCounterCriteria : SecurityCodeNonQueryCommand
    {
        public UpdateMainAccountSecurityCodeCounterCriteria(
            int custId,
            bool checkedSecurityCode,
            string ip,
            string language) : base(custId, ip, language)
        {
            CheckedSecurityCode = checkedSecurityCode;
        }

        public bool CheckedSecurityCode { get; }

        public override string GetSettingKey()
            => "Age_UpdateMainAccountSecurityCodeCounter";
    }
}
