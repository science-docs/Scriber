using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scriber.Drawing;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Logging;
using Scriber.Bibliography;
using Scriber.Localization;
using Scriber.Variables;

namespace Scriber
{
    public class Document : AbstractElement
    {
        public List<DocumentPage> Pages { get; } = new List<DocumentPage>();

        public PageNumberingModule PageNumbering { get; } = new PageNumberingModule();

        public ElementCollection<DocumentElement> Elements { get; }

        public ElementCollection<DocumentElement> PageItems { get; }

        public List<Outline> Outlines { get; } = new List<Outline>();

        public Measurements Measurements { get; }

        public IArrangingStrategy ArrangingStrategy { get; set; } = new DefaultArrangingStrategy();

        public Culture Culture { get; set; } = Culture.Invariant;

        public Locale Locale => new Locale(Culture);

        public Citations? Citations { get; set; }

        public string? Title { get; set; }
        public string? Author { get; set; }

        public Document()
        {
            //Locale = new Locale(Culture);
            Elements = new ElementCollection<DocumentElement>(this);
            PageItems = new ElementCollection<DocumentElement>(this);
            Measurements = new Measurements();
            
            Font = Text.Font.Default;
            FontSize = 12;
        }

        public T Variable<T>(DocumentLocal<T> local)
        {
            return local[this];
        }

        public void Run()
        {
            Run(null);
        }

        public void Run(Logger? logger)
        {
            logger?.Info("First measuring");
            Measure();
            logger?.Info("First arranging");
            Arrange();
            logger?.Info("Interlude");
            Interlude();
            logger?.Info("Second measuring");
            Measure();
            logger?.Info("Second arranging");
            Arrange();
        }

        public override void Interlude()
        {
            foreach (var element in Elements.AsParallel())
            {
                element.Interlude();
            }

            Measurements.Clear();
            Pages.Clear();
            PageNumbering.Reset();
        }

        public void Measure()
        {
            var boxSize = Variable(PageVariables.BoxSize);

            foreach (var element in Elements.AsParallel())
            {
                element.Document = this;
                Measurements.Add(element.Measure(boxSize));
            }

            foreach (var element in PageItems)
            {
                element.Document = this;
            }
        }

        public void Arrange()
        {
            ArrangingStrategy.Arrange(this);
        }

        public byte[] ToPdf()
        {
            var doc = new PdfDocument();
            doc.Info.Author = Author ?? string.Empty;
            doc.Info.Title = Title ?? string.Empty;
            doc.Info.Creator = "Scriber " + typeof(Document).Assembly.GetName().Version?.ToString();

            var renderContext = new PdfDrawingContext
            {
                Document = doc
            };

            foreach (var page in Pages)
            {
                if (page is MeasuredDocumentPage)
                {
                    var pdfPage = doc.AddPage();
                    var graphics = XGraphics.FromPdfPage(pdfPage);
                    renderContext.Graphics = graphics;
                }
                page.OnRender(renderContext);
            }

            if (doc.PageCount == 0)
            {
                doc.AddPage();
            }

            Outline.AddPdf(Outlines, doc);

            using var ms = new MemoryStream();
            doc.Save(ms);
            return ms.ToArray();
        }

        public byte[] ToHtml()
        {
            throw new NotImplementedException();
        }

        protected override AbstractElement CloneInternal()
        {
            throw new NotSupportedException();
        }
    }
}
