using System;
using System.Linq;

namespace Scriber.Bibliography
{
    public struct NumberVariable : INumberVariable
    {
        public NumberVariable(uint value)
        {
            Min = value;
            Max = value;
            Separator = ' ';
        }
        public NumberVariable(uint min, uint max, char separator)
        {
            Min = min;
            Max = max;
            Separator = separator;
        }

        public static bool TryParse(string value, out NumberVariable variable)
        {
            // number?
            if (TryParseNumber(value, out var single))
            {
                variable = new NumberVariable(single);
                return true;
            }

            return TrySplitNumber(value, ",", out variable) 
                || TrySplitNumber(value, "&", out variable) 
                || TrySplitNumber(value, "--", out variable);
        }

        public static bool TryParseOrdinal(string value, out NumberVariable variable)
        {
            if (TryParseNumber(value, out var intValue))
            {
                variable = new NumberVariable(intValue);
                return true;
            }

            uint result = value.ToLowerInvariant() switch
            {
                "first" => 1,
                "second" => 2,
                "third" => 3,
                "fourth" => 4,
                "fifth" => 5,
                "sixth" => 6,
                "seventh" => 7,
                "eigth" => 8,
                "ninth" => 9,
                "tenth" => 10,
                _ => 0
            };

            variable = new NumberVariable(result);
            return result > 0;
        }

        private static bool TryParseNumber(string text, out uint value)
        {
            return uint.TryParse(text, out value);
        }

        private static bool TrySplitNumber(string text, string separators, out NumberVariable variable)
        {
            // init
            NumberVariable? result = null;

            // split
            var parts = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            // 2 parts?
            if (parts.Length == 2)
            {
                // done
                if (TryParseNumber(parts[0], out var n1) && TryParseNumber(parts[1], out var n2))
                {
                    result = new NumberVariable(n1, n2, separators.First());
                }
            }

            variable = result ?? new NumberVariable();

            // done
            return result != null;
        }

        public uint Min { get; }
        public uint Max { get; }
        public char Separator { get; }

        public override string ToString()
        {
            return Min == Max ? Min.ToString() :$"{Min}{Separator}{Max}";
        }
    }
}