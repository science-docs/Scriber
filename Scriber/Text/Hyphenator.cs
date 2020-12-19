using Scriber.Localization;
using System.Collections.Generic;
using System.IO;

namespace Scriber.Text
{
    public class Hyphenator
    {
        public const string Symbol = "\\-";
        
        private readonly NHyphenator.Hyphenator hyphenator;
        private readonly AssemblyPatternsLoader loader;

        private static readonly Dictionary<string, Hyphenator> hyphenators = new Dictionary<string, Hyphenator>();

        private readonly Dictionary<string, string[]> cache = new Dictionary<string, string[]>();

        private Hyphenator(string culture)
        {
            loader = new AssemblyPatternsLoader(culture);
            hyphenator = new NHyphenator.Hyphenator(loader, Symbol, 5, 3, false, culture.StartsWith("de"));
        }

        public static Hyphenator FromCulture(string culture)
        {
            lock (hyphenators)
            {
                if (hyphenators.TryGetValue(culture, out Hyphenator? hyph))
                {
                    return hyph;
                }
                else
                {
                    hyph = new Hyphenator(culture);
                    hyphenators[culture] = hyph;
                    return hyph;
                }
            }
        }

        public string[] Hyphenate(string value)
        {
            if (cache.TryGetValue(value, out var hyphenated))
            {
                return hyphenated;
            }
            else
            {
                var hyphenatedText = hyphenator.HyphenateText(value);
                hyphenated = hyphenatedText.Split(Symbol);
                lock (cache)
                {
                    cache[value] = hyphenated;
                }
                return hyphenated;
            }
        }

        private class AssemblyPatternsLoader : NHyphenator.Loaders.IHyphenatePatternsLoader
        {
            public string LangCode { get; }

            public AssemblyPatternsLoader(string langCode)
            {
                LangCode = langCode.ToLowerInvariant();
            }

            public string LoadExceptions()
            {
                return GetResourceStringContent(GetResourcePath(LangCode, "hyp"));
            }

            public string LoadPatterns()
            {
                return GetResourceStringContent(GetResourcePath(LangCode, "pat"));
            }

            private string GetResourcePath(string langCode, string pat)
            {
                if (langCode == "de" || langCode == "de-de")
                {
                    langCode = "de-1996";
                }
                return $"Scriber.Resources.Hyphenator.hyph-{langCode}.{pat}.txt";
            }

            private string GetResourceStringContent(string path)
            {
                var asm = typeof(Hyphenator).Assembly;
                using var stream = asm.GetManifestResourceStream(path);

                if (stream == null)
                {
                    return string.Empty;
                }

                using var sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }
    }
}
