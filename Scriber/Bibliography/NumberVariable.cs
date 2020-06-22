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

        private static bool TryParseNumber(string text, out uint value)
        {
            return uint.TryParse(text, out value);
        }
        private static bool TrySplitNumber(string text, string separators, out NumberVariable variable)
        {
            // init
            NumberVariable? result = null;

            // split
            var parts = text
                .Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            // 2 parts?
            if (parts.Length == 2)
            {
                // done
                if (TryParseNumber(parts[0], out var n1) && TryParseNumber(string.Format("{0}{1}", parts[0].Substring(0, Math.Max(parts[0].Length - parts[1].Length, 0)), parts[1]), out var n2))
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