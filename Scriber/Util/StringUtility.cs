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
            return char.ToUpperInvariant(c) + value[1..];
        }

        /// <summary>
        /// Transforms css property names into Scriber style keys e.g. <c>margin-bottom</c> into <c>MarginBottom</c>.
        /// Does nothing if the property name is already in a Scriber style key.
        /// </summary>
        /// <param name="kebapCase"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string kebapCase)
        {
            var splits = kebapCase.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return string.Join("", splits.Select(e => e.ToFirstUpper()));
        }

        public static string ToKebapCase(this string pascalCase)
        {
            var splits = pascalCase.Split(c => char.IsUpper(c), true, StringSplitOptions.RemoveEmptyEntries);
            return string.Join('-', splits.Select(e => e.ToLowerInvariant()));
        }

        public static string[] Split(this string value, Func<char, bool> predicate)
        {
            return value.Split(predicate, false, StringSplitOptions.None);
        }

        public static string[] Split(this string value, Func<char, bool> predicate, bool keepSplitChar)
        {
            return value.Split(predicate, keepSplitChar, StringSplitOptions.None);
        }

        public static string[] Split(this string value, Func<char, bool> predicate, StringSplitOptions splitOptions)
        {
            return value.Split(predicate, false, splitOptions);
        }

        public static string[] Split(this string value, Func<char, bool> predicate, bool keepSplitChar, StringSplitOptions splitOptions)
        {
            var splits = new List<string>();
            var span = value.AsSpan();

            int add = keepSplitChar ? 0 : 1;
            int last = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (predicate(value[i]))
                {
                    var split = span[last..i];
                    last = i + add;
                    if (splitOptions != StringSplitOptions.RemoveEmptyEntries || split.Length != 0)
                    {
                        splits.Add(split.ToString());
                    }
                }
            }

            var lastSplit = span[last..];
            if (splitOptions != StringSplitOptions.RemoveEmptyEntries || lastSplit.Length != 0)
            {
                splits.Add(lastSplit.ToString());
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
