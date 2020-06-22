using Scriber.Bibliography.Styling.Specification;
using System;
using System.Linq;

namespace Scriber.Localization
{
    public class Locale
    {
        public Culture Culture { get; }

        public LocaleFile File { get; }
        
        public Locale(Culture culture)
        {
            Culture = Culture;
            File = LocaleFile.Defaults.FirstOrDefault(e => e.XmlLang == culture.ToString());
        }

        public Locale(LocaleFile file)
        {
            Culture = new Culture(file.XmlLang);
            File = file;
        }

        public string OpenQuote => GetTerm(TermName.OpenQuote) ?? "\"";
        public string CloseQuote => GetTerm(TermName.CloseQuote) ?? "\"";

        public string FormatNumber(double value)
        {
            return value.ToString(Culture.GetCultureInfo());
        }

        public string FormatNumber(double value, NumberFormat format)
        {
            return format switch
            {
                NumberFormat.Numeric => FormatNumber(value),
                NumberFormat.Roman => FormatRomanNumber((int)value),
                NumberFormat.LongOrdinal => FormatOrdinal((int)value, true),
                NumberFormat.Ordinal => FormatOrdinal((int)value, false),
                _ => string.Empty
            };
        }

        private static string[][] _RomanNumerals = new string[][]
            {
                new string[]{"", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix"}, // ones
                new string[]{"", "x", "xx", "xxx", "xl", "l", "lx", "lxx", "lxxx", "xc"}, // tens
                new string[]{"", "c", "cc", "ccc", "cd", "d", "dc", "dcc", "dccc", "cm"}, // hundreds
                new string[]{"", "m", "mm", "mmm"} // thousands
            };

        private string FormatRomanNumber(int number)
        {
            if (number == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (number >= 4000)
            {
                throw new ArgumentOutOfRangeException();
            }

            // done
            return string.Join("", number.ToString()
                .ToCharArray()
                .Reverse()
                .Select((c, i) => _RomanNumerals[i][int.Parse(c.ToString())])
                .Reverse());
        }

        private string FormatOrdinal(int value, bool _long)
        {
            var term = GetOrdinalNumberTerm(value, _long);
            string? termValue = GetTerm(term);

            if (termValue == null)
            {
                return FormatNumber(value);
            }
            else if (_long)
            {
                return termValue;
            }
            else
            {
                return value + termValue;
            }
        }

        private TermName GetOrdinalNumberTerm(int value, bool _long)
        {
            string name = $"{(_long ? "long-" : string.Empty)}ordinal-{value:00}";
            if (Enum.TryParse<TermName>(name, out var term))
            {
                return term;
            }
            else
            {
                return TermName.Ordinal;
            }
        }

        public string? GetTerm(TermName name, TermFormat? format = null, bool? single = null, Gender? gender = null)
        {
            Func<TermElement, bool> comparer;

            if (format != null && gender != null)
            {
                comparer = e => e.Name == name && e.FormatSpecified && e.Format == format && e.Gender == gender;
            }
            else if (format != null)
            {
                comparer = e => e.Name == name && e.FormatSpecified && e.Format == format;
            }
            else
            {
                comparer = e => e.Name == name;
            }

            var term = File.Terms?.FirstOrDefault(e => comparer(e));

            if (term != null)
            {
                if (term.Value != null)
                {
                    return term.Value;
                }
                else if (single == null || single.Value)
                {
                    return term.Single;
                }
                else
                {
                    return term.Multiple;
                }
            }

            return null;
        }
    }
}
