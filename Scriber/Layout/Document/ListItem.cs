using Scriber.Drawing;
using System;

namespace Scriber.Layout.Document
{
    public enum ListStyle
    {
        Disc = 0,
        Circle,
        Square,
        None,
        Decimal,
        DecimalLeadingZero,
        LowerRoman,
        UpperRoman,
        LowerAlpha,
        UpperAlpha
    }

    public class ListItem : Paragraph
    {
        private const double Buffer = 24;

        public int Index { get; set; }
        public ListStyle Style { get; set; }

        public ListItem(Paragraph content)
        {
            Leaves.AddRange(content.Leaves);
        }

        protected override AbstractElement CloneInternal()
        {
            throw new NotSupportedException();
            //return new ListItem(base.ne());
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            availableSize.Width -= Buffer;
            return base.MeasureOverride(availableSize);
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            drawingContext.Offset = new Position(10, 0);
            drawingContext.DrawText(new TextRun(GetPreamble(), new Text.Typeface(Font!, FontSize, FontWeight, FontStyle)), Foreground);

            drawingContext.PushTransform(new TranslateTransform(new Position(Buffer, 0)));

            base.OnRender(drawingContext, measurement);

            drawingContext.PopTransform();
        }

        public string GetPreamble()
        {
            return Style switch
            {
                ListStyle.Disc => "•",
                ListStyle.Circle => "○",
                ListStyle.Square => "■",
                ListStyle.Decimal => Index.ToString() + ".",
                ListStyle.DecimalLeadingZero => Index.ToString("00") + ".",
                ListStyle.LowerAlpha => GetChar(Index, 'a') + ".",
                ListStyle.UpperAlpha => GetChar(Index, 'A') + ".",
                _ => ""
            };

            static string GetChar(int index, char start)
            {
                return ((char)(index + start - 1)).ToString();
            }
        }
    }
}
