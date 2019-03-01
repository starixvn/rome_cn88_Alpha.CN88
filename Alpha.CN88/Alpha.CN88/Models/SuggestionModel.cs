namespace Alpha.CN88.Models
{
    public class SuggestionModel
    {
        public SuggestionModel(string username, string firstName, string lastName)
        {
            Username = username;
            Firstname = firstName;
            Lastname = lastName;
        }

        public string Username { get; }

        public string Firstname { get; }

        public string Lastname { get; }
    }
}