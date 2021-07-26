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

        private static readonly Dictionary<string, IStyleKey> keys = new();

        public static StyleKey<Font> Font { get; } = new(nameof(Font), true, Text.Font.Default);
        public static StyleKey<Unit> FontSize { get; } = new(nameof(FontSize), true, Unit.FromPoint(12));

        public static StyleKey<FontWeight> FontWeight { get; } = new(nameof(FontWeight), true, Text.FontWeight.Normal);
        public static StyleKey<FontStyle> FontStyle { get; } = new(nameof(FontStyle), true, Text.FontStyle.Normal);

        public static ComputedStyleKey<Typeface> Typeface { get; } = new(nameof(Typeface), style => 
        { 
            var font = style.Get(Font)!; 
            var size = style.Get(FontSize).Point; 
            var weight = style.Get(FontWeight); 
            var fontStyle = style.Get(FontStyle); 
            return new Typeface(font, size, weight, fontStyle); 
        });

        public static StyleKey<HorizontalAlignment> HorizontalAlignment { get; } = new(nameof(HorizontalAlignment), Layout.HorizontalAlignment.Justify);
        public static StyleKey<VerticalAlignment> VerticalAlignment { get; } = new(nameof(VerticalAlignment), Layout.VerticalAlignment.Top);

        public static StyleKey<Unit> MarginTop { get; } = new(nameof(MarginTop), Unit.Zero);
        public static StyleKey<Unit> MarginRight { get; } = new(nameof(MarginRight), Unit.Zero);
        public static StyleKey<Unit> MarginBottom { get; } = new(nameof(MarginBottom), Unit.Zero);
        public static StyleKey<Unit> MarginLeft { get; } = new(nameof(MarginLeft), Unit.Zero);

        public static ThicknessStyleKey Margin { get; } = new(nameof(Margin), MarginTop, MarginLeft, MarginBottom, MarginRight);

        public static StyleKey<Unit> PaddingTop { get; } = new(nameof(PaddingTop), Unit.Zero);
        public static StyleKey<Unit> PaddingRight { get; } = new(nameof(PaddingRight), Unit.Zero);
        public static StyleKey<Unit> PaddingBottom { get; } = new(nameof(PaddingBottom), Unit.Zero);
        public static StyleKey<Unit> PaddingLeft { get; } = new(nameof(PaddingLeft), Unit.Zero);

        public static ThicknessStyleKey Padding { get; } = new(nameof(Padding), PaddingTop, PaddingLeft, PaddingBottom, PaddingRight);

        public static StyleKey<Color> BorderColor { get; } = new(nameof(BorderColor), true, Colors.Black);

        public static StyleKey<Unit> BorderTop { get; } = new(nameof(BorderTop), Unit.Zero);
        public static StyleKey<Unit> BorderRight { get; } = new(nameof(BorderRight), Unit.Zero);
        public static StyleKey<Unit> BorderBottom { get; } = new(nameof(BorderBottom), Unit.Zero);
        public static StyleKey<Unit> BorderLeft { get; } = new(nameof(BorderLeft), Unit.Zero);

        public static ThicknessStyleKey Border { get; } = new(nameof(Border), BorderTop, BorderLeft, BorderBottom, BorderRight);

        public static ComputedStyleKey<Thickness> FullMargin { get; } = new(nameof(FullMargin), style => style.Get(Margin) + style.Get(Border) + style.Get(Padding));

        public static StyleKey<int> RowSpan { get; } = new(nameof(RowSpan), 1);
        public static StyleKey<int> ColumnSpan { get; } = new(nameof(ColumnSpan), 1);

        // Paragraph styles
        public static StyleKey<Unit> BaselineSkip { get; } = new(nameof(BaselineSkip), true, Unit.FromPoint(-1));
        public static StyleKey<double> BaselineStretch { get; } = new(nameof(BaselineStretch), true, 1.25);
        public static StyleKey<Unit> ParagraphIndent { get; } = new(nameof(ParagraphIndent), true, Unit.FromPoint(0));
    }
}
