using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public abstract class MeasuringBlock : DocumentElement
    {
        public abstract void Manipulate(Scriber.Document document);

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            if (Document != null)
            {
                Manipulate(Document);
            }
            return new Measurement(this);
        }

        public override void Interlude()
        {
            Invalidate();
        }
    }
}
