using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class StyleKey<T> : AbstractStyleKey<T>
    {
        [NotNull]
        public T Default { get; }

        public StyleKey(string name, T defaultValue) : this(name, false, defaultValue)
        {
        }

        public StyleKey(string name, bool inherited, T defaultValue) : base(name, inherited)
        {
            Default = defaultValue;
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
