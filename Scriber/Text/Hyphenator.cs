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

        private readonly Dictionary<string, string[]> cache = new Dictionary<string, string[]>();

        public Hyphenator(Culture culture)
        {
            loader = new AssemblyPatternsLoader(culture.ToString());
            hyphenator = new NHyphenator.Hyphenator(loader, Symbol);
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
                cache[value] = hyphenated;
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
