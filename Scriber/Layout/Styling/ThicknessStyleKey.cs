using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public class ThicknessStyleKey : AbstractStyleKey<Thickness>
    {
        public StyleKey<Unit> Top { get; }
        public StyleKey<Unit> Left { get; }
        public StyleKey<Unit> Bottom { get; }
        public StyleKey<Unit> Right { get; }

        public ThicknessStyleKey(string name, StyleKey<Unit> top, StyleKey<Unit> left, StyleKey<Unit> bottom, StyleKey<Unit> right) : base(name, false)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        [return: NotNull]
        public override Thickness Get(Style style)
        {
            var topValue = style.Get(Top).Point;
            var leftValue = style.Get(Left).Point;
            var bottomValue = style.Get(Bottom).Point;
            var rightValue = style.Get(Right).Point;
            return new Thickness(topValue, leftValue, bottomValue, rightValue);
        }

        public override void Set(StyleContainer style, [DisallowNull] Thickness value)
        {
            style.Set(Top, Unit.FromPoint(value.Top));
            style.Set(Left, Unit.FromPoint(value.Right));
            style.Set(Bottom, Unit.FromPoint(value.Bottom));
            style.Set(Right, Unit.FromPoint(value.Left));
        }
    }
}
