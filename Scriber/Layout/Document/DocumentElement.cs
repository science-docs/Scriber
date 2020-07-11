using System;
using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public abstract class DocumentElement : AbstractElement
    {
        public Measurement Measurement { get; private set; }
        public Transform? Transform { get; set; }
        public virtual bool IsVisible => true;

        protected DocumentElement()
        {
            Measurement = new Measurement(this);
        }

        public Measurement Measure(Size availableSize)
        {
            return Measurement = MeasureOverride(availableSize);
        }

        public void Arrange(Measurement finalMeasurement)
        {
            ArrangeOverride(finalMeasurement);
        }

        protected abstract Measurement MeasureOverride(Size availableSize);

        protected virtual void ArrangeOverride(Measurement finalMeasurement)
        {

        }

        public virtual SplitResult Split(Measurement source, double height)
        {
            var measurement = new Measurement(this);
            Measurement? next = new Measurement(this);
            double carryOver = -1;

            foreach (var sub in source.Subs)
            {
                if (sub.Position.Y + sub.Size.Height >= height)
                {
                    if (carryOver == -1)
                    {
                        carryOver = height - sub.Position.Y;
                    }

                    var pos = sub.Position;
                    pos.Y -= height;
                    pos.Y += carryOver;
                    sub.Position = pos;
                    next.Subs.Add(sub);
                }
                else
                {
                    measurement.Subs.Add(sub);
                }
            }

            if (next.Subs.Count == 0)
            {
                next = null;
            }

            return new SplitResult(source, measurement, next);
        }

        public void Render(IDrawingContext drawingContext, Measurement measurement)
        {
            drawingContext.PushTransform(new TranslateTransform(measurement.Position));

            if (Transform != null)
            {
                drawingContext.PushTransform(Transform);
            }

            OnRender(drawingContext, measurement);

            if (Transform != null)
            {
                drawingContext.PopTransform();
            }

            drawingContext.PopTransform();
        }

        protected abstract void OnRender(IDrawingContext drawingContext, Measurement measurement);

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
