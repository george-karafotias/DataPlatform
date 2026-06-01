using Data.Core.Greeklish.Interfaces;
using System.Text;

namespace Data.Core.Greeklish.Services;

public class GreeklishConverter : IGreeklishConverter
{
    // 1. Strong dictionary (high confidence)
    private static readonly Dictionary<string, string> Dictionary = new()
    {
        { "kalimera", "καλημέρα" },
        { "kalispera", "καλησπέρα" },
        { "kaneis", "κάνεις" },
        { "eimai", "είμαι" },
        { "ti", "τι" },
        { "sas", "σας" },
        { "kali", "καλή" }
    };

    // 2. Phonetic rules (fallback layer)
    private static readonly Dictionary<string, string> Map = new()
    {
        { "th", "θ" },
        { "ch", "χ" },
        { "ps", "ψ" },
        { "ks", "ξ" },
        { "ou", "ου" },
        { "ai", "αι" },
        { "ei", "ει" },
        { "oi", "οι" },

        { "a", "α" },
        { "b", "β" },
        { "g", "γ" },
        { "d", "δ" },
        { "e", "ε" },
        { "z", "ζ" },
        { "h", "η" },
        { "i", "ι" },
        { "k", "κ" },
        { "l", "λ" },
        { "m", "μ" },
        { "n", "ν" },
        { "x", "ξ" },
        { "o", "ο" },
        { "p", "π" },
        { "r", "ρ" },
        { "s", "σ" },
        { "t", "τ" },
        { "y", "υ" },
        { "f", "φ" },
        { "w", "ω" }
    };

    public string Convert(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.ToLowerInvariant();

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();

        foreach (var word in words)
        {
            var cleaned = Clean(word);

            // 1. EXACT MATCH (best)
            if (Dictionary.TryGetValue(cleaned, out var exact))
            {
                result.Add(exact);
                continue;
            }

            // 2. FUZZY MATCH (smart layer)
            var fuzzy = FindBestFuzzyMatch(cleaned);
            if (fuzzy != null)
            {
                result.Add(fuzzy);
                continue;
            }

            // 3. PHONETIC FALLBACK
            result.Add(ConvertPhonetically(cleaned));
        }

        return string.Join(" ", result);
    }

    // ----------------------------
    // PHONETIC CONVERTER
    // ----------------------------
    private string ConvertPhonetically(string word)
    {
        var sb = new StringBuilder();
        int i = 0;

        while (i < word.Length)
        {
            if (i + 1 < word.Length)
            {
                var duo = word.Substring(i, 2);

                if (Map.TryGetValue(duo, out var val))
                {
                    sb.Append(val);
                    i += 2;
                    continue;
                }
            }

            var single = word[i].ToString();

            if (Map.TryGetValue(single, out var s))
                sb.Append(s);
            else
                sb.Append(word[i]);

            i++;
        }

        return sb.ToString();
    }

    // ----------------------------
    // FUZZY MATCHING (CORE SMARTNESS)
    // ----------------------------
    private string? FindBestFuzzyMatch(string word)
    {
        string? best = null;
        int bestScore = int.MaxValue;

        foreach (var entry in Dictionary)
        {
            int distance = Levenshtein(word, entry.Key);

            if (distance < bestScore)
            {
                bestScore = distance;
                best = entry.Value;
            }
        }

        // threshold prevents wrong matches
        return bestScore <= 2 ? best : null;
    }

    // ----------------------------
    // LEVENSHTEIN DISTANCE
    // ----------------------------
    private int Levenshtein(string a, string b)
    {
        if (string.IsNullOrEmpty(a)) return b.Length;
        if (string.IsNullOrEmpty(b)) return a.Length;

        var dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++)
            dp[i, 0] = i;

        for (int j = 0; j <= b.Length; j++)
            dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = a[i - 1] == b[j - 1] ? 0 : 1;

                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1,
                             dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        }

        return dp[a.Length, b.Length];
    }

    // ----------------------------
    // NORMALIZATION
    // ----------------------------
    private string Clean(string word)
    {
        return word.Trim()
                   .ToLowerInvariant();
    }
}