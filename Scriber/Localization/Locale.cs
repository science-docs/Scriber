using Scriber.Bibliography.Styling.Specification;
using Scriber.Util;
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
            Culture = culture;
            File = LocaleFile.Defaults.First(e => e.XmlLang == culture.ToString());
        }

        public Locale(LocaleFile file)
        {
            Culture = Culture.GetCulture(file.XmlLang!);
            File = file;
        }

        public string OpenQuote => GetTerm(TermName.OpenQuote) ?? "\"";
        public string CloseQuote => GetTerm(TermName.CloseQuote) ?? "\"";

        public string GetTerm(Term term)
        {
            if (Culture.Language == "en")
            {
                return term.ToString();
            }
            else if (Culture.Language == "de")
            {
                return term switch
                {
                    Term.Figure => "Abbildung",
                    Term.Listing => "Listing",
                    Term.Table => "Tabelle",
                    _ => ""
                };
            }
            else
            {
                return term.ToString();
            }
        }

        public string FormatNumber(double value)
        {
            return value.ToString(Culture.GetCultureInfo());
        }

        public string FormatNumber(double value, NumberFormat format)
        {
            return FormatNumber(value, format, null);
        }

        public string FormatNumber(double value, NumberFormat format, Gender? gender)
        {
            return format switch
            {
                NumberFormat.Numeric => FormatNumber(value),
                NumberFormat.Roman => FormatRomanNumber((int)value),
                NumberFormat.LongOrdinal => FormatOrdinal((int)value, true, gender),
                NumberFormat.Ordinal => FormatOrdinal((int)value, false, gender),
                _ => string.Empty
            };
        }

        private static readonly string[][] _RomanNumerals = new string[][]
            {
                new string[]{"", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix"}, // ones
                new string[]{"", "x", "xx", "xxx", "xl", "l", "lx", "lxx", "lxxx", "xc"}, // tens
                new string[]{"", "c", "cc", "ccc", "cd", "d", "dc", "dcc", "dccc", "cm"}, // hundreds
                new string[]{"", "m", "mm", "mmm"} // thousands
            };

        public static string FormatRomanNumber(int number)
        {
            if (number == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }
            else if (number >= 4000)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            // done
            return string.Join("", number.ToString()
                .ToCharArray()
                .Reverse()
                .Select((c, i) => _RomanNumerals[i][int.Parse(c.ToString())])
                .Reverse());
        }

        private string FormatOrdinal(int value, bool isLong, Gender? gender)
        {
            var term = GetOrdinalNumberTerm(value, isLong);
            string? termValue = GetTerm(term, null, null, gender);

            if (termValue == null)
            {
                return FormatNumber(value);
            }
            else if (isLong)
            {
                return termValue;
            }
            else
            {
                return value + termValue;
            }
        }

        private static TermName GetOrdinalNumberTerm(int value, bool isLong)
        {
            string name = $"{(isLong ? "long-" : string.Empty)}ordinal-{value:00}";
            if (EnumUtility.TryParseEnum<TermName>(name, out var term))
            {
                return term;
            }
            else
            {
                return TermName.Ordinal;
            }
        }

        public Gender? GetTermGender(TermName name)
        {
            var term = File.Terms.FirstOrDefault(e => e.Name == name);
            if (term != null && term.GenderSpecified)
            {
                return term.Gender;
            }
            else
            {
                return null;
            }
        }

        public string? GetTerm(TermName name, TermFormat? format = null, bool? single = null, Gender? gender = null)
        {
            Func<TermElement, bool> comparer;

            if (format != null && gender != null)
            {
                comparer = e => e.Name == name && e.Format == format && e.Gender == gender;
            }
            else if (format != null)
            {
                comparer = e => e.Name == name && e.Format == format;
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
