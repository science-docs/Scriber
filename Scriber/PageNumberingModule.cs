using Scriber.Localization;

namespace Scriber
{
    public enum PageNumberingStyle
    {
        Arabic,
        Roman,
        RomanUpper,
        Alphabet,
        AlphabetUpper,
    }

    public class PageNumberingModule
    {
        public int Current
        {
            get => current;
            set
            {
                if (value < 1)
                    value = 1;
                current = value;
            }
        }
        public PageNumberingStyle Style { get; set; } = PageNumberingStyle.Arabic;

        private int current = 1;

        public void Reset()
        {
            current = 1;
            Style = PageNumberingStyle.Arabic;
        }

        public string Next()
        {
            var value = Current++;

            return Style switch
            {
                PageNumberingStyle.Arabic => value.ToString(),
                PageNumberingStyle.Alphabet => ((char)(value + 'a' - 1)).ToString(),
                PageNumberingStyle.AlphabetUpper => ((char)(value + 'A' - 1)).ToString(),
                PageNumberingStyle.Roman => Locale.FormatRomanNumber(value),
                PageNumberingStyle.RomanUpper => Locale.FormatRomanNumber(value).ToUpperInvariant(),
                _ => value.ToString(),
            };
        }
    }
}
