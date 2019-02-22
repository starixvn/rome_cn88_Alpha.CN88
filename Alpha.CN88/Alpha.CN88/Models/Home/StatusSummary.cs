namespace Alpha.CN88.Models.Home
{
    public class StatusSummary
    {
        public StatusSummary()
        {
        }

        public StatusSummary(string name, int active, int inactive, int viewOnly, int suspened)
        {
            Name = name;
            Active = active;
            Inactive = inactive;
            ViewOnly = viewOnly;
            Suspened = suspened;
        }

        public string Name { get; }

        public int Active { get; private set; }

        public int Inactive { get; private set; }

        public int ViewOnly { get; private set; }

        public int Suspened { get; private set; }

        public void Add(StatusSummary s)
        {
            Active += s.Active;
            Inactive += s.Inactive;
            ViewOnly += s.ViewOnly;
            Suspened += s.Suspened;
        }
    }
}
