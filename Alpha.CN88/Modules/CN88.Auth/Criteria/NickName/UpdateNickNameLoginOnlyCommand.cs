namespace CN88.Auth.Criteria.NickName
{
    public class UpdateNickNameLoginOnlyCommand : NickNameNonQueryCommand
    {
        public UpdateNickNameLoginOnlyCommand(
            int custId,
            bool nickNameLoginOnly,
            string ip,
            string language) : base(custId, ip, language)
        {
            NickNameLoginOnly = nickNameLoginOnly;
        }

        public bool NickNameLoginOnly { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdNicknameLoginOnly";
    }
}
