using Scriber.Util;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public abstract class AbstractStyleKey<T> : IStyleKey, IEquatable<AbstractStyleKey<T>>
    {
        public string Name { get; }

        public bool Inherited { get; }

        public Type Type { get; }

        protected AbstractStyleKey(string name, bool inherited)
        {
            Name = name;
            Inherited = inherited;
            Type = typeof(T);
            StyleKeys.Add(this);
        }

        [return: NotNull]
        public abstract T Get(Style style);
        public abstract void Set(StyleContainer style, [DisallowNull] T value);

        public bool Equals([AllowNull] AbstractStyleKey<T> other)
        {
            return Name == other?.Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is AbstractStyleKey<T> styleKey)
            {
                return Equals(styleKey);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }

        public override string ToString()
        {
            return $"{Name}<{Type.FormattedName()}>";
        }

        object IStyleKey.Get(Style style)
        {
            return Get(style);
        }

        void IStyleKey.Set(StyleContainer styleContainer, object value)
        {
            if (value is T genericValue)
            {
                Set(styleContainer, genericValue);
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}
