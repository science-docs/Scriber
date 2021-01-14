using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    /// <summary>
    /// Base class for block element that manipulate the document during the measuring phase.
    /// </summary>
    public abstract class ArrangingBlock : DocumentElement
    {
        public abstract void Manipulate(Scriber.Document document);

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {

        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return new Measurement(this);
        }

        protected override void ArrangeOverride(Measurement finalMeasurement)
        {
            if (Document != null)
            {
                Manipulate(Document);
            }
        }

        public override void Interlude()
        {
            Invalidate();
        }
    }

    public abstract class MeasuringBlock : DocumentElement
    {
        public abstract void Manipulate(Scriber.Document document);

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
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
