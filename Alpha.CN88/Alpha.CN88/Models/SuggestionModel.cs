namespace Alpha.CN88.Models
{
    public class SuggestionModel
    {
        public SuggestionModel(string username, string firstName, string lastName, string username2)
        {
            Username = username;
            Firstname = firstName;
            Lastname = lastName;
            Username2 = username2;
        }

        public string Username { get; }

        public string Firstname { get; }

        public string Lastname { get; }

        public string Username2 { get; }
    }
}