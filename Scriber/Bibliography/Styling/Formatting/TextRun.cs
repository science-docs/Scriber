using System.Collections.Generic;
using System.Diagnostics;

namespace Scriber.Bibliography.Styling.Formatting
{
    [DebuggerDisplay("{Text}")]
    public sealed class TextRun : Run
    {
        public TextRun(string text, FontStyle fontStyle, FontVariant fontVariant, FontWeight fontWeight, TextDecoration textDecoration, VerticalAlign verticalAlign)
            : base(string.IsNullOrEmpty(text))
        {
            // init
            Text = text;
            FontStyle = fontStyle;
            FontVariant = fontVariant;
            FontWeight = fontWeight;
            TextDecoration = textDecoration;
            VerticalAlign = verticalAlign;
        }

        public string Text { get; private set; }
        public FontStyle FontStyle { get; }
        public FontVariant FontVariant { get; }
        public FontWeight FontWeight { get; }
        public TextDecoration TextDecoration { get; }
        public VerticalAlign VerticalAlign { get; }

        public bool IsFormatEqual(TextRun other)
        {
            return FontStyle == other.FontStyle &&
                FontVariant == other.FontVariant &&
                FontWeight == other.FontWeight &&
                TextDecoration == other.TextDecoration &&
                VerticalAlign == other.VerticalAlign;
        }

        internal override IEnumerable<TextRun> GetTextRuns()
        {
            yield return this;
        }
    }
}
