using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class StyleKey<T> : AbstractStyleKey<T>
    {
        [NotNull]
        public T Default { get; }

        public StyleKey(string name, [NotNull] T defaultValue) : this(name, false, defaultValue)
        {
        }

        public StyleKey(string name, bool inherited, [NotNull] T defaultValue) : base(name, inherited)
        {
            Default = defaultValue ?? throw new InvalidOperationException();
        }

        [return: NotNull]
        public override T Get(Style style)
        {
            return style.Get(Name, Inherited, Default) ?? Default;
        }

        public override void Set(StyleContainer style, [DisallowNull] T value)
        {
            style.Set(Name, value);
        }
    }
}
