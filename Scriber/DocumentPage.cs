using Scriber.Drawing;
using Scriber.Layout;

namespace Scriber
{
    public abstract class DocumentPage
    {
        public Document Document { get; }
        public Size Size { get; set; }
        public Rectangle ContentArea { get; set; }
        public string? Number { get; set; }
        public int Index { get; set; }

        public DocumentPage(Document document)
        {
            Document = document;
        }

        public abstract void OnRender(IDrawingContext drawingContext);
    }
}
