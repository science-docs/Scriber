using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class ComputedStyleKey<T> : AbstractStyleKey<T>
    {
        public Func<Style, T> Getter { get; set; }
        public Action<StyleContainer, T>? Setter { get; set; }

        public ComputedStyleKey(string name, Func<Style, T> getter) : this(name, getter, null)
        {
        }

        public ComputedStyleKey(string name, Func<Style, T> getter, Action<StyleContainer, T>? setter) : this(name, false, getter, setter)
        {
        }

        public ComputedStyleKey(string name, bool inherited, Func<Style, T> getter, Action<StyleContainer, T>? setter) : base(name, inherited)
        {
            Getter = getter;
            Setter = setter;
        }

        [return: NotNull]
        public override T Get(Style style)
        {
            return Getter(style) ?? throw new InvalidOperationException();
        }

        public override void Set(StyleContainer style, [DisallowNull] T value)
        {
            Setter?.Invoke(style, value);
        }
    }
}
