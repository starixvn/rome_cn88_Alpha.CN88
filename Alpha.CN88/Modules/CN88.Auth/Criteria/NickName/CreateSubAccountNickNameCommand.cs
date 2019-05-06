namespace CN88.Auth.Criteria.NickName
{
    using Params.NickName;

    public class CreateSubAccountNickNameCommand : CreateNickNameCommand
    {
        public CreateSubAccountNickNameCommand(CreateNickNameParam param)
            : base(param)
        {
            SubAccId = param.SubAccountId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "CS_CustSubInsNickname";
    }
}
