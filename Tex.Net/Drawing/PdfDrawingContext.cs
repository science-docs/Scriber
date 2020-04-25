using PdfSharpCore.Drawing;
using System.Drawing;
using Tex.Net.Layout;
using Tex.Net.Text;

namespace Tex.Net.Drawing
{
    public class PdfDrawingContext : IDrawingContext
    {
        public XGraphics Graphics { get; set; }
        public Position Offset { get; set; }

        public void DrawImage(Image image, Layout.Rectangle rectangle)
        {
            Graphics.DrawImage(XImage.FromStream(() => image.GetStream()), new XRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));
        }

        public void DrawText(TextRun run, Color color)
        {
            Graphics.DrawString(run.Text, ToXFont(run.Typeface.Font, run.Typeface.Size), ToXBrush(color), new XPoint(Offset.X, Offset.Y));
        }

        private XFont ToXFont(Font font, double size)
        {
            var xFont = new XFont(font.Name, size, XFontStyle.Regular);
            return xFont;
        }

        private XBrush ToXBrush(Color color)
        {
            return new XSolidBrush(XColor.FromArgb(color.ToArgb()));
        }
    }
}
