using System;
using System.Collections.Generic;
using Scripts.Model.Data;

namespace Scripts.Utils
{
    public static class DialogLocalizationParser
    {
        public static void Parse(string fileContent, List<DialogSentence> sentences)
        {
            if (string.IsNullOrEmpty(fileContent))
                return;

            var lines = fileContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split('\t');

                if (parts.Length < 2)
                    continue;
                
                var sentence = parts[0].Trim();
                var isHeroSpeaking = string.Equals(parts[1].Trim(), "true", StringComparison.OrdinalIgnoreCase);

                sentences.Add(new DialogSentence
                {
                    Sentence = sentence,
                    IsHeroSpeaking = isHeroSpeaking
                });
            }
        }
    }
}