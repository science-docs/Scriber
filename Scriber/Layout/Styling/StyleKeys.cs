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
        }, (styleContainer, typeface) =>
        {

        });

        public static StyleKey<HorizontalAlignment> HorizontalAlignment { get; } = new StyleKey<HorizontalAlignment>(nameof(HorizontalAlignment), true, Layout.HorizontalAlignment.Justify);
        public static StyleKey<VerticalAlignment> VerticalAlignment { get; } = new StyleKey<VerticalAlignment>(nameof(VerticalAlignment), true, Layout.VerticalAlignment.Top);

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
        }, (styleContainer, margin) =>
        {
            styleContainer.Set(MarginTop, Unit.FromPoint(margin.Top));
            styleContainer.Set(MarginRight, Unit.FromPoint(margin.Right));
            styleContainer.Set(MarginBottom, Unit.FromPoint(margin.Bottom));
            styleContainer.Set(MarginLeft, Unit.FromPoint(margin.Left));
        });

        // Paragraph styles
        public static StyleKey<Unit?> BaselineSkip { get; } = new StyleKey<Unit?>(nameof(BaselineSkip), true, null);
        public static StyleKey<double> BaselineStretch { get; } = new StyleKey<double>(nameof(BaselineStretch), true, 1.25);
        //public static StyleKey<Unit> ParagraphSkip { get; } = new StyleKey<Unit>(nameof(ParagraphSkip), true, Unit.FromPoint(8));
        public static StyleKey<Unit> ParagraphIndent { get; } = new StyleKey<Unit>(nameof(ParagraphIndent), true, Unit.FromPoint(0));
    }
}
