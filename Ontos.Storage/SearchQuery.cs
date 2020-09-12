using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Storage
{
    public class SearchQuery
    {
        private const int MIN_AUTOCOMPLETE_LENGTH = 3;
        private const int MIN_FUZZY_LENGTH = 5;
        public string QueryText { get; }

        public SearchQuery(string queryText)
        {
            QueryText = queryText;
        }

        public string Autocomplete()
        {
            var words = QueryText.Split(' ');

            for (int i = 0; i < words.Length - 1; i++)
                if (words[i].Length > MIN_FUZZY_LENGTH)
                    words[i] += "~1";

            if (words[^1].Length >= MIN_AUTOCOMPLETE_LENGTH)
                words[^1] += "*";

            return string.Join(" ", words);
        }
    }
}
