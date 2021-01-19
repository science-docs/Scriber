using Scriber.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class StyleKey : IEquatable<StyleKey>
    {
        public static bool TryGetStyleKey(string name, [MaybeNullWhen(false)] out StyleKey styleKey)
        {
            return keys.TryGetValue(name, out styleKey);
        }

        private static readonly Dictionary<string, StyleKey> keys = new Dictionary<string, StyleKey>();

        public string Name { get; }
        public bool Inherited { get; } = false;
        public object? Default { get; }
        public Type Type { get; }

        public StyleKey(string name, Type type, object? defaultValue) : this(name, type, false, defaultValue)
        {
        }

        public StyleKey(string name, Type type, bool inherited, object? defaultValue)
        {
            Name = name;
            Type = type;
            Default = defaultValue;
            Inherited = inherited;
            keys.Add(name, this);
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

        public override string ToString()
        {
            return $"{Name}<{Type.FormattedName()}>";
        }
    }
}
