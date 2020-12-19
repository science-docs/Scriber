using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Language
{
    public struct TextSpan : IEquatable<TextSpan>
    {
        public int Start { get; }
        public int Length { get; }
        public int Line { get; }
        public int End => Start + Length;

        public TextSpan(int start, int length, int line)
        {
            Start = start;
            Length = length;
            Line = line;
        }

        public TextSpan WithEnd(int end)
        {
            return new TextSpan(Start, end - Start, Line);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TextSpan textSpan)
            {
                return Equals(textSpan);
            }
            else
            {
                return false;
            }
        }

        public bool Equals([AllowNull] TextSpan other)
        {
            return Start == other.Start && Length == other.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length);
        }

        public static bool operator ==(TextSpan left, TextSpan right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextSpan left, TextSpan right)
        {
            return !(left == right);
        }
    }
}
