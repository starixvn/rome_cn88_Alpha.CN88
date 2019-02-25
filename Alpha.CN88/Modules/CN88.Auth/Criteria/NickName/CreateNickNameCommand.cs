namespace CN88.Auth.Criteria.NickName
{
    public class CreateNickNameCommand : NickNameNonQueryCommand
    {
        public CreateNickNameCommand(
            int custId,
            string nickName,
            string ip,
            string language) : base(custId, ip, language)
        {
            NickName = nickName;
        }

        public string NickName { get; }

        public override string GetSettingKey()
            => "Age_CustomerInsNickName";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(NickName);
    }
}
