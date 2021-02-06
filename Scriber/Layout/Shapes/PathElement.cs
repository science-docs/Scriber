using Scriber.Drawing;
using Scriber.Drawing.Shapes;
using Scriber.Layout.Document;
using Scriber.Layout.Styling;
using Scriber.Text;
using System;

namespace Scriber.Layout.Shapes
{
    class PathElement : DocumentElement
    {
        public Path Path { get; set; }

        public Color Fill { get; set; }
        public Color Stroke { get; set; }
        public double StrokeWidth { get; set; } = 0.5;

        public PathElement(Path path)
        {
            Path = path;
        }

        protected override AbstractElement CloneInternal()
        {
            return new PathElement(Path);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return new Measurement(this, Path.Size, Style.Get(StyleKeys.Margin));
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            drawingContext.DrawPath(Path, Fill, new Pen(Stroke, StrokeWidth));
        }
    }
}
