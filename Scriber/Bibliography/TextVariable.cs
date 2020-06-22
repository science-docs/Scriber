using System;

namespace Scriber.Bibliography
{
    public struct TextVariable : ITextVariable, IEquatable<TextVariable>, IEquatable<ITextVariable>
    {
        public string Value { get; }

        public TextVariable(string value)
        {
            Value = value;
        }

        public static implicit operator TextVariable(string value) => new TextVariable(value);
        public static implicit operator string(TextVariable value) => value.Value;

        public static bool operator ==(TextVariable a, TextVariable b) => a.Equals(b);
        public static bool operator !=(TextVariable a, TextVariable b) => !(a == b);
        public override bool Equals(object? obj) => obj is TextVariable variable && Equals(variable);
        public bool Equals(TextVariable other) => Value == other.Value;
        public bool Equals(ITextVariable? other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public bool IsNullOrEmpty => string.IsNullOrEmpty(Value);

        public override string ToString() => Value;
    }
}