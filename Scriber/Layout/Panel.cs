using Scriber.Drawing;
using Scriber.Layout.Document;
using System;

namespace Scriber.Layout
{
    public abstract class Panel : DocumentElement
    {
        public ElementCollection<DocumentElement> Elements { get; }
        public bool Flexible { get; set; }
        public bool Glue { get; set; }

        protected Panel()
        {
            Elements = new ElementCollection<DocumentElement>(this);
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            foreach (var sub in measurement.Subs)
            {
                sub.Element.Render(drawingContext, sub);
            }
        }

        public override void Interlude()
        {
            foreach (var child in Elements)
            {
                child.Interlude();
            }
        }
    }
}
