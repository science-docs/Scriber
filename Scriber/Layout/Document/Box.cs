using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class Box : DocumentElement
    {
        public Size Size { get; }

        public Box(Size size)
        {
            Size = size;
        }

        public override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            // Is only spacing and does not render anything
        }

        protected override AbstractElement CloneInternal()
        {
            return new Box(Size);
        }

        protected override Measurements MeasureOverride(Size availableSize)
        {
            return Measurements.Singleton(new Measurement(this, Size, Margin));
        }
    }
}
