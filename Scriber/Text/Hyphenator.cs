using System.IO;

namespace Scriber.Text
{
    public class Hyphenator
    {
        public const string Symbol = "\\-";
        
        private readonly NHyphenator.Hyphenator hyphenator;
        private readonly AssemblyPatternsLoader loader;

        public Hyphenator(string langCode)
        {
            loader = new AssemblyPatternsLoader(langCode);
            hyphenator = new NHyphenator.Hyphenator(loader, Symbol);
        }

        public string[] Hyphenate(string value)
        {
            var hyphenated = hyphenator.HyphenateText(value);
            return hyphenated.Split(Symbol);
        }

        private class AssemblyPatternsLoader : NHyphenator.Loaders.IHyphenatePatternsLoader
        {
            public string LangCode { get; }

            public AssemblyPatternsLoader(string langCode)
            {
                LangCode = langCode;
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
                return $"Tex.Net.Resources.Hyphenator.hyph-{langCode}.{pat}.txt";
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
