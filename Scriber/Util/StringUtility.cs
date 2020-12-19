using System;
using System.Collections.Generic;

namespace Scriber.Util
{
    public static class StringUtility
    {
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

        public static string Trim(this string value, out int advance)
        {
            return TrimStart(value, out advance).TrimEnd();
        }

        public static string TrimStart(this string value, out int advance)
        {
            var trimmed = value.TrimStart();
            advance = value.Length - trimmed.Length;
            return trimmed;
        }

        public static string Pluralify(this int count, string single, string plural)
        {
            return count == 1 ? single : plural;
        }
    }
}
