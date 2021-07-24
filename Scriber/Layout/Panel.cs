using Scriber.Drawing;
using Scriber.Layout.Document;
using Scriber.Text;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout
{
    public abstract class Panel : DocumentElement
    {
        public ElementCollection<DocumentElement> Elements { get; }
        public bool Flexible { get; set; }

        public override IEnumerable<Symbol> Symbols => Elements.SelectMany(e => e.Symbols);

        protected Panel()
        {
            Elements = new ElementCollection<DocumentElement>(this);
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            return Elements;
        }

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            foreach (var sub in measurement.Subs)
            {
                sub.Element.Render(drawingContext, sub);
            }
        }

        protected override void ArrangeOverride(Measurement finalMeasurement)
        {
            foreach (var sub in finalMeasurement.Subs)
            {
                sub.Element.Arrange(sub);
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
