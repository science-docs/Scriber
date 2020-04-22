using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Drawing;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;

namespace Tex.Net
{
    public class DocumentPage
    {
        public Measurements Measurements { get; }
        public Document Document { get; }
        public Size Size { get; set; }
        public Rectangle ContentArea { get; set; }

        public string Number { get; set; }

        public DocumentPage(Document document)
        {
            Document = document;
            Measurements = new Measurements();
        }

        public void OnRender(IDrawingContext drawingContext)
        {
            foreach (var measurement in Measurements)
            {
                drawingContext.Offset = measurement.Position;
                measurement.Element.OnRender(drawingContext, measurement);
            }
        }
    }
}
