using PdfSharpCore.Pdf;
using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber
{
    public class Outline
    {
        public string Content { get; set; }
        public AbstractElement Target { get; set; }
        public int? Page => Target?.Page?.Index;
        public List<Outline> Children { get; } = new List<Outline>();

        public Outline(string content, AbstractElement target)
        {
            Content = content;
            Target = target;
        }

        public static void AddPdf(IEnumerable<Outline> outlines, PdfDocument doc)
        {
            AddPdf(outlines, doc, doc.Outlines);
        }

        public static void AddPdf(IEnumerable<Outline> outlines, PdfDocument doc, PdfOutlineCollection pdfOutlines)
        {
            foreach (var outline in outlines)
            {
                var pageIndex = outline.Page;
                if (pageIndex != null)
                {
                    var page = doc.Pages[pageIndex.Value];
                    var pdfOutline = new PdfOutline(outline.Content, page);
                    pdfOutlines.Add(pdfOutline);
                    AddPdf(outline.Children, doc, pdfOutline.Outlines);
                }
            }
        }
    }
}
