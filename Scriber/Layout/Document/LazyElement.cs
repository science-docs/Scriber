using Scriber.Drawing;
using Scriber.Text;
using System;
using System.Collections.Generic;

namespace Scriber.Layout.Document
{
    public class LazyElement : DocumentElement
    {
        public Func<DocumentElement> Factory { get; }

        public DocumentElement Value
        {
            get
            {
                if (value == null)
                {
                    value = Factory();
                    value.Parent = Parent;
                }
                return value;
            }
        }

        public override IEnumerable<Symbol> Symbols => Value.Symbols;

        private DocumentElement? value;

        public LazyElement(Func<DocumentElement> factory)
        {
            Factory = factory;
        }

        protected override AbstractElement CloneInternal()
        {
            return new LazyElement(Factory);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return Value.Measure(availableSize);
        }

        protected override void ArrangeOverride(Measurement finalMeasurement)
        {
            Value.Arrange(finalMeasurement);
        }

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            Value.OnRender(drawingContext, measurement);
        }

        public override void Interlude()
        {
            Value.Interlude();
        }

        public override void Invalidate()
        {
            Value.Invalidate();
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            return Value.ChildElements();
        }

        public override SplitResult Split(Measurement source, double height)
        {
            return Value.Split(source, height);
        }
    }
}
