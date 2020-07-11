using Scriber.Drawing;
using Scriber.Layout;

namespace Scriber
{
    public class DocumentPage
    {
        public Measurements Measurements { get; }
        public Document Document { get; }
        public Size Size { get; set; }
        public Rectangle ContentArea { get; set; }

        public bool Filled { get; set; }
        public string? Number { get; set; }

        public DocumentPage(Document document)
        {
            Document = document;
            Measurements = new Measurements();
        }

        public void AddPageItems()
        {
            foreach (var pageItem in Document.PageItems)
            {
                var clone = pageItem.Clone();
                clone.Parent = Document;
                clone.Page = this;
                clone.Interlude();
                var measures = clone.Measure(Size);
                Measurements.Add(measures);
            }
        }

        public void OnRender(IDrawingContext drawingContext)
        {
            foreach (var measurement in Measurements)
            {
                drawingContext.Offset = measurement.Position;
                measurement.Element.Render(drawingContext, measurement);

                foreach (var accumulatedSub in measurement.AccumulatedExtra.Subs)
                {
                    accumulatedSub.Element.Render(drawingContext, accumulatedSub);
                }
            }
        }
    }
}
