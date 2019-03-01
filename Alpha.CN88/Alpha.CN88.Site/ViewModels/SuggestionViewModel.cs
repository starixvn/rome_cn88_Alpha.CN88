namespace Alpha.CN88.Site.ViewModels
{
    using System.Collections.Generic;

    public class SuggestionViewModel
    {
        public SuggestionViewModel(string query, IEnumerable<string[]> suggestions)
        {
            this.query = query;
            this.suggestions = suggestions;
        }

        public string query { get; }

        public IEnumerable<string[]> suggestions { get; }
    }
}