namespace Alpha.CN88.Models.Home
{
    public class StatusSummary
    {
        public StatusSummary()
        {
        }

        public byte RoleId { get; }

        public string Name => UserLevel.FromValue<UserLevel>(RoleId).DisplayNameInMultipleLanguage;

        public int Active { get; private set; }

        public int Inactive { get; private set; }

        public int Suspened { get; private set; }

        public void Add(StatusSummary s)
        {
            Active += s.Active;
            Inactive += s.Inactive;
            Suspened += s.Suspened;
        }
    }
}
