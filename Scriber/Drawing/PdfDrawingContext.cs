using PdfSharpCore.Drawing;
using Scriber.Layout;
using Scriber.Text;
using System;

namespace Scriber.Drawing
{
    public class PdfDrawingContext : DrawingContext
    {
        public XGraphics? Graphics
        {
            get => graphics;
            set
            {
                graphics = value;
                graphics?.MultiplyTransform(XMatrix.Identity);
                ClearTransform();
            }
        }

        private XGraphics G => Graphics ?? throw new InvalidOperationException("Cannot use pdf drawing context without setting the Graphics property");
        private XGraphics? graphics;

        public override void DrawImage(Image image, Rectangle rectangle)
        {
            var tr = rectangle.Transform(GetTransform());
            G.DrawImage(XImage.FromStream(() => image.GetStream()), new XRect(tr.X, tr.Y, tr.Width, tr.Height));
        }

        public override void DrawText(TextRun run, Color color)
        {
            double yOffset = FontStyler.ScaleOffset(run.Typeface.Style, run.Typeface.Size);
            double size = FontStyler.ScaleSize(run.Typeface.Style, run.Typeface.Size);
            var transformedOffset = Offset.Transform(GetTransform());
            G.DrawString(run.Text, ToXFont(run.Typeface.Font, size, run.Typeface.Weight), ToXBrush(color), new XPoint(transformedOffset.X, transformedOffset.Y + yOffset));
        }

        private XFont ToXFont(Font font, double size, FontWeight weight)
        {
            // Quick convert via integer casting
            var xWeight = (XFontStyle)(int)weight;
            var xFont = new XFont(font.Name, size, xWeight);
            return xFont;
        }

        private XBrush ToXBrush(Color color)
        {
            return new XSolidBrush(XColor.FromArgb(color.Argb));
        }

        public override void AddLink(Rectangle rectangle, int targetPage)
        {
            //Graphics.PdfPage.AddDocumentLink(new PdfSharpCore.Pdf.PdfRectangle())
        }
    }
}
