﻿using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    /// <summary>
    /// Base class for block element that manipulate the document during the measuring phase.
    /// </summary>
    public abstract class ArrangingBlock : DocumentElement
    {
        public override bool IsVisible => false;

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
    }
}
