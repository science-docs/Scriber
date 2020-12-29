using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Util
{
    public static class StringUtility
    {
        public static string ToFirstUpper(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var c = value[0];
            return char.ToUpperInvariant(c) + value.Substring(1);
        }

        /// <summary>
        /// Transforms css property names into Scriber style keys e.g. <c>margin-bottom</c> into <c>MarginBottom</c>.
        /// Does nothing if the property name is already in a Scriber style key.
        /// </summary>
        /// <param name="styleProperty"></param>
        /// <returns></returns>
        public static string TransformStyleProperty(this string styleProperty)
        {
            var splits = styleProperty.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return string.Join("", splits.Select(e => e.ToFirstUpper()));
        }

        public static string[] Split(this string value, Func<char, bool> predicate)
        {
            return value.Split(predicate, StringSplitOptions.None);
        }

        public static string[] Split(this string value, Func<char, bool> predicate, StringSplitOptions splitOptions)
        {
            var splits = new List<string>();

            int last = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (predicate(value[i]))
                {
                    var split = value[last..i];
                    last = i + 1;
                    if (splitOptions != StringSplitOptions.RemoveEmptyEntries || !string.IsNullOrEmpty(split))
                    {
                        splits.Add(split);
                    }
                }
            }

            var lastSplit = value[last..];
            if (splitOptions != StringSplitOptions.RemoveEmptyEntries || !string.IsNullOrEmpty(lastSplit))
            {
                splits.Add(lastSplit);
            }

            return splits.ToArray();
        }

        public static bool IsWord(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsLetterOrDigit(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumber(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsDigit(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string Pluralify(this int count, string single, string plural)
        {
            return count == 1 ? single : plural;
        }
    }
}
