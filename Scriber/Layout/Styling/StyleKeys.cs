using Scriber.Text;

namespace Scriber.Layout.Styling
{
    public static class StyleKeys
    {
        public static StyleKey<Font> Font { get; } = new StyleKey<Font>(nameof(Font), true, Text.Font.Default);
        public static StyleKey<Unit> FontSize { get; } = new StyleKey<Unit>(nameof(FontSize), true, Unit.FromPoint(12));

        public static StyleKey<double> MarginTop { get; } = new StyleKey<double>(nameof(MarginTop), 0);
        public static StyleKey<double> MarginRight { get; } = new StyleKey<double>(nameof(MarginRight), 0);
        public static StyleKey<double> MarginBottom { get; } = new StyleKey<double>(nameof(MarginBottom), 0);
        public static StyleKey<double> MarginLeft { get; } = new StyleKey<double>(nameof(MarginLeft), 0);

        public static ComputedStyleKey<Thickness> Margin { get; } = new ComputedStyleKey<Thickness>(nameof(Margin), style =>
        {
            var top = style.Get(MarginTop);
            var right = style.Get(MarginRight);
            var bottom = style.Get(MarginBottom);
            var left = style.Get(MarginLeft);
            return new Thickness(top, left, bottom, right);
        });


        // Paragraph styles
        public static StyleKey<Unit?> BaselineSkip { get; } = new StyleKey<Unit?>(nameof(BaselineSkip), true, null);
        public static StyleKey<double> BaselineStretch { get; } = new StyleKey<double>(nameof(BaselineStretch), true, 1.25);
        //public static StyleKey<Unit> ParagraphSkip { get; } = new StyleKey<Unit>(nameof(ParagraphSkip), true, Unit.FromPoint(8));
        public static StyleKey<Unit> ParagraphIndent { get; } = new StyleKey<Unit>(nameof(ParagraphIndent), true, Unit.FromPoint(0));
    }
}
