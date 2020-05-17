using System;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public abstract class DocumentElement : AbstractElement
    {
        public Measurements Measurements { get; private set; } = new Measurements();
        public virtual bool IsVisible => true;

        public Measurements Measure(Size availableSize)
        {
            return Measurements = MeasureOverride(availableSize);
        }

        public void Arrange(Measurement finalMeasurement)
        {
            ArrangeOverride(finalMeasurement);
        }

        protected abstract Measurements MeasureOverride(Size availableSize);

        protected virtual void ArrangeOverride(Measurement finalMeasurement)
        {

        }

        public abstract void OnRender(IDrawingContext drawingContext, Measurement measurement);

        public new DocumentElement Clone()
        {
            if (!(base.Clone() is DocumentElement element))
            {
                throw new InvalidCastException();
            }

            return element;
        }
    }
}
