using Scriber.Text;

namespace Scriber.Layout.Styling
{
    public static class StyleKeys
    {
        public static StyleKey<Font> Font { get; } = new StyleKey<Font>(nameof(Font), true, Text.Font.Default);
        public static StyleKey<Unit> FontSize { get; } = new StyleKey<Unit>(nameof(FontSize), true, Unit.FromPoint(12));

        public static StyleKey<Unit> MarginTop { get; } = new StyleKey<Unit>(nameof(MarginTop), Unit.Zero);
        public static StyleKey<Unit> MarginRight { get; } = new StyleKey<Unit>(nameof(MarginRight), Unit.Zero);
        public static StyleKey<Unit> MarginBottom { get; } = new StyleKey<Unit>(nameof(MarginBottom), Unit.Zero);
        public static StyleKey<Unit> MarginLeft { get; } = new StyleKey<Unit>(nameof(MarginLeft), Unit.Zero);

        public static ComputedStyleKey<Thickness> Margin { get; } = new ComputedStyleKey<Thickness>(nameof(Margin), style =>
        {
            var top = style.Get(MarginTop).Point;
            var right = style.Get(MarginRight).Point;
            var bottom = style.Get(MarginBottom).Point;
            var left = style.Get(MarginLeft).Point;
            return new Thickness(top, left, bottom, right);
        });


        // Paragraph styles
        public static StyleKey<Unit?> BaselineSkip { get; } = new StyleKey<Unit?>(nameof(BaselineSkip), true, null);
        public static StyleKey<double> BaselineStretch { get; } = new StyleKey<double>(nameof(BaselineStretch), true, 1.25);
        //public static StyleKey<Unit> ParagraphSkip { get; } = new StyleKey<Unit>(nameof(ParagraphSkip), true, Unit.FromPoint(8));
        public static StyleKey<Unit> ParagraphIndent { get; } = new StyleKey<Unit>(nameof(ParagraphIndent), true, Unit.FromPoint(0));
    }
}
