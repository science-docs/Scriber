﻿using System;
using Tex.Net.Drawing;

namespace Tex.Net.Layout.Document
{
    public abstract class DocumentElement : AbstractElement
    {
        public Measurements Measurements { get; private set; }
        public virtual bool IsVisible => true;

        public Measurements Measure(Size availableSize)
        {
            return Measurements = MeasureOverride(availableSize);
        }

        protected abstract Measurements MeasureOverride(Size availableSize);

        public abstract void OnRender(IDrawingContext drawingContext, Measurement measurement);

        public new DocumentElement Clone()
        {
            return base.Clone() as DocumentElement;
        }
    }
}
