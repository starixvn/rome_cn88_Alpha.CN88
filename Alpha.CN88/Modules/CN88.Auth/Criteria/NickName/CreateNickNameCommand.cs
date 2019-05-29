namespace CN88.Auth.Criteria.NickName
{
    using Params.NickName;

    public class CreateNickNameCommand : NickNameNonQueryCommand
    {
        public CreateNickNameCommand(CreateNickNameParam param)
            : base(param.CustId, param.IP, param.LanguageKey)
        {
            NickName = param.NickName;
        }

        public string NickName { get; }

        public override string GetSettingKey()
            => "CS_CustomerInsNickName";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(NickName);
    }
}
