using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Scriber.Drawing.Shapes;
using Scriber.Layout;
using Scriber.Text;
using System;
using System.Numerics;

namespace Scriber.Drawing
{
    public class PdfDrawingContext : DrawingContext
    {
        public PdfDocument? Document { get; set; }

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

        public override void DrawPath(Path path, Color fill, Color stroke, double strokeWidth)
        {
            var state = G.Save();
            G.MultiplyTransform(ToXMatrix(GetTransform()));
            var xpath = path.ToPdfPath();
            G.DrawPath(ToXPen(stroke, strokeWidth), ToXBrush(fill), xpath);
            G.Restore(state);
        }

        public override void DrawText(TextRun run, Color color)
        {
            double yOffset = FontStyler.ScaleOffset(run.Typeface.Style, run.Typeface.Size);
            double size = FontStyler.ScaleSize(run.Typeface.Style, run.Typeface.Size);
            var transformedOffset = Offset.Transform(GetTransform());
            G.DrawString(run.Text, run.Typeface.Font.GetXFont(size, run.Typeface.Weight), ToXBrush(color), new XPoint(transformedOffset.X, transformedOffset.Y + yOffset));
        }

        private XBrush ToXBrush(Color color)
        {
            return new XSolidBrush(XColor.FromArgb(color.Argb));
        }

        private XPen ToXPen(Color color, double strokeWidth)
        {
            return new XPen(ToXBrush(color), strokeWidth);
        }

        private XMatrix ToXMatrix(Matrix3x2 matrix)
        {
            return new XMatrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32);
        }

        public override void AddLink(Rectangle rectangle, int targetPage)
        {
            var transform = GetTransform();
            rectangle.Position = rectangle.Position.Transform(transform);
            var rect = G.Transformer.WorldToDefaultPage(new XRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));
            G.PdfPage.AddDocumentLink(new PdfRectangle(rect), targetPage + 1);
        }

        
    }
}
