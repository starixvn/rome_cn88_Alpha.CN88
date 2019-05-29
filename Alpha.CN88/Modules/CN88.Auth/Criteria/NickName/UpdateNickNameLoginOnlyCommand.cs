namespace CN88.Auth.Criteria.NickName
{
    using Params.NickName;

    public class UpdateNickNameLoginOnlyCommand : NickNameNonQueryCommand
    {
        public UpdateNickNameLoginOnlyCommand(CreateNickNameParam param, bool nickNameLoginOnly)
            : base(param.CustId, param.IP, param.LanguageKey)
        {
            NickNameLoginOnly = nickNameLoginOnly;
        }

        public bool NickNameLoginOnly { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdNicknameLoginOnly";
    }
}
