using System.Collections.Generic;

namespace Scripts.Utils
{
    public static class PopupLocalizationParser
    {
        public static Dictionary<string, string> Parse(string fileContent)
        {
            var dictionary = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(fileContent))
                return dictionary;

            var rows = fileContent.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var row in rows)
            {
                var parts = row.Split('\t');

                if (parts.Length < 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}