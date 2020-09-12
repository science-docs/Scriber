using Scriber.Drawing;
using Scriber.Layout.Document;

namespace Scriber
{
    public class PdfDocumentPage : DocumentPage
    {
        public PdfElement Element { get; }

        public PdfDocumentPage(Document document, PdfElement element) : base(document)
        {
            Element = element;
        }

        public override void OnRender(IDrawingContext drawingContext)
        {
            Element.Render(drawingContext);
        }
    }
}
