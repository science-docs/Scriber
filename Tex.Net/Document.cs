using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using Tex.Net.Drawing;
using Tex.Net.Layout.Document;

namespace Tex.Net
{
    public class Document : DocumentElement
    {
        public DocumentVariable Variables { get; } = new DocumentVariable();

        public List<DocumentPage> Pages { get; } = new List<DocumentPage>();

        public DocumentElementCollection<Block> Elements { get; }

        public Document()
        {
            Elements = new DocumentElementCollection<Block>(this);
            Font = Text.Font.Serif;
            FontSize = 12;
        }

        public byte[] ToPdf()
        {
            var doc = new PdfDocument();
            var renderContext = new PdfDrawingContext();

            foreach (var page in Pages)
            {
                var pdfPage = doc.AddPage();
                var graphics = XGraphics.FromPdfPage(pdfPage);
                renderContext.Graphics = graphics;
                page.OnRender(renderContext);
            }

            using var ms = new MemoryStream();
            doc.Save(ms);
            return ms.ToArray();
        }

        public byte[] ToHtml()
        {
            throw new NotImplementedException();
        }
    }
}
