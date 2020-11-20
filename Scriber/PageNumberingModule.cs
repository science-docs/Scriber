using Scriber.Localization;
using System;

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
                PageNumberingStyle.Alphabet => AlphaBase(value),
                PageNumberingStyle.AlphabetUpper => AlphaBase(value).ToUpperInvariant(),
                PageNumberingStyle.Roman => Locale.FormatRomanNumber(value),
                PageNumberingStyle.RomanUpper => Locale.FormatRomanNumber(value).ToUpperInvariant(),
                _ => value.ToString(),
            };
        }

        private static readonly long[] Base26 = new long[7];

        static PageNumberingModule()
        {
            Base26[0] = 1;
            for (int i = 1; i < Base26.Length; i++)
            {
                Base26[i] = Base26[i - 1] * 26;
            }
        }

        private static string AlphaBase(long value)
        {
            if (value > Base26[6] || value < 1)
            {
                return string.Empty;
            }

            int index = 0;
            while (index < 6 && Base26[index + 1] < value)
            {
                index++;
            }
            var item = Base26[index];
            var times = value / item;
            var result = times * item;
            return ((char)('a' + times - 1)).ToString() + AlphaBase(value - result);
        }
    }
}
