using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Scriber.Drawing.Shapes;
using Scriber.Layout;
using Scriber.Text;
using System;

namespace Scriber.Drawing.Pdf
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
            G.DrawImage(XImage.FromStream(() => image.GetStream()), tr.ToXRect());
        }

        public override void DrawPath(Path path, Color fill, Pen pen)
        {
            var state = G.Save();
            G.MultiplyTransform(GetTransform().ToXMatrix());
            var xpath = path.ToPdfPath();
            G.DrawPath(pen.ToXPen(), fill.ToXBrush(), xpath);
            G.Restore(state);
        }

        public override void DrawLine(Position start, Position end, Pen pen)
        {
            var transform = GetTransform();
            G.DrawLine(pen.ToXPen(), start.Transform(transform).ToXPoint(), end.Transform(transform).ToXPoint());
        }

        public override void DrawRectangle(Rectangle rect, Color fill, Pen pen)
        {
            var tr = rect.Transform(GetTransform());
            G.DrawRectangle(pen.ToXPen(), fill.ToXBrush(), tr.ToXRect());
        }

        public override void DrawText(TextRun run, Color color)
        {
            double yOffset = FontStyler.ScaleOffset(run.Typeface.Style, run.Typeface.Size);
            double size = FontStyler.ScaleSize(run.Typeface.Style, run.Typeface.Size);
            var transformedOffset = Offset.Transform(GetTransform());
            G.DrawString(run.Text, run.Typeface.Font.GetXFont(size, run.Typeface.Weight), color.ToXBrush(), new XPoint(transformedOffset.X, transformedOffset.Y + yOffset));
        }

        public override void AddLink(Rectangle rectangle, int targetPage)
        {
            var transform = GetTransform();
            rectangle.Position = rectangle.Position.Transform(transform);
            var rect = G.Transformer.WorldToDefaultPage(rectangle.ToXRect());
            G.PdfPage.AddDocumentLink(new PdfRectangle(rect), targetPage + 1);
        }
    }
}
