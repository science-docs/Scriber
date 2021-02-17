using Scriber.Drawing;
using Scriber.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Layout.Styling
{
    public static class StyleKeys
    {
        public static bool TryGetByName(string name, [MaybeNullWhen(false)] out IStyleKey styleKey)
        {
            return keys.TryGetValue(name, out styleKey);
        }

        internal static void Add(IStyleKey key)
        {
            keys.Add(key.Name, key);
        }

        private static readonly Dictionary<string, IStyleKey> keys = new Dictionary<string, IStyleKey>();

        public static StyleKey<Font> Font { get; } = new StyleKey<Font>(nameof(Font), true, Text.Font.Default);
        public static StyleKey<Unit> FontSize { get; } = new StyleKey<Unit>(nameof(FontSize), true, Unit.FromPoint(12));

        public static StyleKey<FontWeight> FontWeight { get; } = new StyleKey<FontWeight>(nameof(FontWeight), true, Text.FontWeight.Normal);
        public static StyleKey<FontStyle> FontStyle { get; } = new StyleKey<FontStyle>(nameof(FontStyle), true, Text.FontStyle.Normal);

        public static ComputedStyleKey<Typeface> Typeface { get; } = new ComputedStyleKey<Typeface>(nameof(Typeface), style =>
        {
            var font = style.Get(Font)!;
            var size = style.Get(FontSize).Point;
            var weight = style.Get(FontWeight);
            var fontStyle = style.Get(FontStyle);
            return new Typeface(font, size, weight, fontStyle);
        });

        public static StyleKey<HorizontalAlignment> HorizontalAlignment { get; } = new StyleKey<HorizontalAlignment>(nameof(HorizontalAlignment), Layout.HorizontalAlignment.Justify);
        public static StyleKey<VerticalAlignment> VerticalAlignment { get; } = new StyleKey<VerticalAlignment>(nameof(VerticalAlignment), Layout.VerticalAlignment.Top);

        public static StyleKey<Unit> MarginTop { get; } = new StyleKey<Unit>(nameof(MarginTop), Unit.Zero);
        public static StyleKey<Unit> MarginRight { get; } = new StyleKey<Unit>(nameof(MarginRight), Unit.Zero);
        public static StyleKey<Unit> MarginBottom { get; } = new StyleKey<Unit>(nameof(MarginBottom), Unit.Zero);
        public static StyleKey<Unit> MarginLeft { get; } = new StyleKey<Unit>(nameof(MarginLeft), Unit.Zero);

        public static ThicknessStyleKey Margin { get; } = new ThicknessStyleKey(nameof(Margin), MarginTop, MarginLeft, MarginBottom, MarginRight);

        public static StyleKey<Unit> PaddingTop { get; } = new StyleKey<Unit>(nameof(PaddingTop), Unit.Zero);
        public static StyleKey<Unit> PaddingRight { get; } = new StyleKey<Unit>(nameof(PaddingRight), Unit.Zero);
        public static StyleKey<Unit> PaddingBottom { get; } = new StyleKey<Unit>(nameof(PaddingBottom), Unit.Zero);
        public static StyleKey<Unit> PaddingLeft { get; } = new StyleKey<Unit>(nameof(PaddingLeft), Unit.Zero);

        public static ThicknessStyleKey Padding { get; } = new ThicknessStyleKey(nameof(Padding), PaddingTop, PaddingLeft, PaddingBottom, PaddingRight);

        public static StyleKey<Color> BorderColor { get; } = new StyleKey<Color>(nameof(BorderColor), true, Colors.Black);

        public static StyleKey<Unit> BorderTop { get; } = new StyleKey<Unit>(nameof(BorderTop), Unit.Zero);
        public static StyleKey<Unit> BorderRight { get; } = new StyleKey<Unit>(nameof(BorderRight), Unit.Zero);
        public static StyleKey<Unit> BorderBottom { get; } = new StyleKey<Unit>(nameof(BorderBottom), Unit.Zero);
        public static StyleKey<Unit> BorderLeft { get; } = new StyleKey<Unit>(nameof(BorderLeft), Unit.Zero);

        public static ThicknessStyleKey Border { get; } = new ThicknessStyleKey(nameof(Border), BorderTop, BorderLeft, BorderBottom, BorderRight);

        public static StyleKey<int> RowSpan { get; } = new StyleKey<int>(nameof(RowSpan), 1);
        public static StyleKey<int> ColumnSpan { get; } = new StyleKey<int>(nameof(ColumnSpan), 1);

        // Paragraph styles
        public static StyleKey<Unit?> BaselineSkip { get; } = new StyleKey<Unit?>(nameof(BaselineSkip), true, null);
        public static StyleKey<double> BaselineStretch { get; } = new StyleKey<double>(nameof(BaselineStretch), true, 1.25);
        //public static StyleKey<Unit> ParagraphSkip { get; } = new StyleKey<Unit>(nameof(ParagraphSkip), true, Unit.FromPoint(8));
        public static StyleKey<Unit> ParagraphIndent { get; } = new StyleKey<Unit>(nameof(ParagraphIndent), true, Unit.FromPoint(0));
    }
}
