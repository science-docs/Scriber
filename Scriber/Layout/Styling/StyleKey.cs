using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class StyleKey<T> : StyleKey
    {
        public StyleKey(string name, T defaultValue) : base(name, defaultValue)
        {
        }

        public StyleKey(string name, bool inherited, T defaultValue) : base(name, inherited, defaultValue)
        {
        }
    }

    public class StyleKey : IEquatable<StyleKey>
    {
        public string Name { get; }
        public bool Inherited { get; } = false;
        public object? Default { get; }

        public StyleKey(string name, object? defaultValue)
        {
            Name = name;
            Default = defaultValue;
        }

        public StyleKey(string name, bool inherited, object? defaultValue)
        {
            Name = name;
            Default = defaultValue;
            Inherited = inherited;
        }

        public bool Equals([AllowNull] StyleKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is StyleKey key)
            {
                return Equals(key);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
