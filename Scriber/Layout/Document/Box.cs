using Scriber.Drawing;
using Scriber.Layout.Styling;

namespace Scriber.Layout.Document
{
    public class Box : DocumentElement
    {
        public Size Size { get; }

        public Box(Size size)
        {
            Size = size;
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            // Is only spacing and does not render anything
        }

        protected override AbstractElement CloneInternal()
        {
            return new Box(Size);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return new Measurement(this, Size, Style.Get(StyleKeys.Margin));
        }
    }
}
