using Scriber.Text;
using System;
using System.Globalization;
using System.Linq;

namespace Scriber.Localization
{
    public class Culture : IEquatable<Culture>
    {
        /// <summary>
        /// The invariant culture.
        /// </summary>
        public static readonly Culture Invariant = new Culture("en-US");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="xmlLang"></param>
        private Culture(string? xmlLang)
        {
            var parts = (xmlLang ?? "").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Language = parts.Length == 0 || string.IsNullOrEmpty(parts[0]) || parts[0].Length != 2 ? throw new ArgumentException("Language part of the culture needs to be two characters long.") : parts[0].ToLower();
            Dialect = parts.Length > 1 && !string.IsNullOrEmpty(parts[1]) && parts[1].Length == 2 && !string.IsNullOrEmpty(Language) ? parts[1].ToUpper() : null;
        }

        public static Culture[] GetCultures()
        {
            return new Culture[]
            {
                new Culture("en-US"),
                new Culture("en-GB"),
                new Culture("de-DE")
            };
        }


        public static Culture GetCulture(string name)
        {
            if (!TryGetCulture(name, out var culture))
            {
                throw new Exception();
            }
            return culture;
        }

        public static bool TryGetCulture(string name, out Culture culture)
        {
            var cultures = GetCultures();

            var cult = cultures.FirstOrDefault(culture => string.Equals(culture.Language, name, StringComparison.CurrentCultureIgnoreCase)
                                             || string.Equals(culture.ToString(), name, StringComparison.CurrentCultureIgnoreCase));

            if (cult == null)
            {
                culture = Invariant;
                return false;
            }
            else
            {
                culture = cult;
                return true;
            }
        }

        public static bool IsCultureLanguage(string cultureName)
        {
            return TryGetCulture(cultureName, out _);
        }

        public Hyphenator GetHyphenator()
        {
            return Hyphenator.FromCulture(ToString());
        }

        /// <summary>
        /// The language of the culture, in lowercase.
        /// </summary>
        public string Language { get; }
        /// <summary>
        /// The dialect of the culture, in uppercase.
        /// </summary>
        public string? Dialect { get; }

        /// <summary>
        /// Indicates whether both language and dialect are set. 
        /// </summary>
        public bool IsDialect
        {
            get
            {
                return !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(Dialect);
            }
        }
        /// <summary>
        /// Indicates whether language is set but dialect is not set. 
        /// </summary>
        public bool IsLanguage
        {
            get
            {
                return !string.IsNullOrEmpty(Language) && string.IsNullOrEmpty(Dialect);
            }
        }
        /// <summary>
        /// Indicates whether both language and dialect are not set. 
        /// </summary>
        public bool IsInvariant
        {
            get
            {
                return string.IsNullOrEmpty(Language) && string.IsNullOrEmpty(Dialect);
            }
        }

        /// <summary>
        /// Implicit conversion operator to string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Culture(string value)
        {
            return new Culture(value);
        }
        /// <summary>
        /// Implicit conversion operator from string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(Culture value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Equal operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Culture? a, Culture? b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a is null) || (b is null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }
        /// <summary>
        /// Inequal operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Culture? a, Culture? b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Culture culture && Equals(culture);
        }
        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Culture? other)
        {
            return other != null && other.Language == Language && other.Dialect == Dialect;
        }
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public CultureInfo GetCultureInfo()
        {
            if (IsInvariant)
            {
                return CultureInfo.InvariantCulture;
            }
            else
            {
                return new CultureInfo(ToString());
            }
        }

        /// <summary>
        /// Returns the full name of this culture.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsInvariant)
            {
                return "";
            }
            else
            {
                return IsLanguage ? Language : $"{Language}-{Dialect}";
            }
        }
    }
}
