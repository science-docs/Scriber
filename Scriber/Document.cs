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
using System.Diagnostics.CodeAnalysis;
using Scriber.Variables;

namespace Scriber
{
    public class Document : AbstractElement
    {
        public List<DocumentPage> Pages { get; } = new List<DocumentPage>();

        public PageNumberingModule PageNumbering { get; } = new PageNumberingModule();

        public ElementCollection<DocumentElement> Elements { get; }

        public ElementCollection<DocumentElement> PageItems { get; }

        public Measurements Measurements { get; }

        public Text.Hyphenator Hyphenator { get; set; }

        public IArrangingStrategy ArrangingStrategy { get; set; } = new DefaultArrangingStrategy();

        public Size PageSize { get; private set; }
        public Size PageBoxSize { get; private set; }
        public Thickness PageMargin { get; private set; }

        public Culture Culture { get; set; } = new Culture("en-US");

        public Locale Locale { get; set; }

        public Citations? Citations { get; set; }

        public Document()
        {
            Locale = new Locale(Culture);
            Elements = new ElementCollection<DocumentElement>(this);
            PageItems = new ElementCollection<DocumentElement>(this);
            Measurements = new Measurements();
            
            Font = Text.Font.Serif;
            FontSize = 11;

            Hyphenator = new Text.Hyphenator("en-us");
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
            PageNumbering.Set(1);
        }

        public void Measure()
        {
            PageSize = Variable(PageVariables.Size);
            PageMargin = Variable(PageVariables.Margin);

            PageBoxSize = new Size(PageSize.Width - PageMargin.Width, PageSize.Height - PageMargin.Height);

            foreach (var element in Elements.AsParallel())
            {
                element.Document = this;
                var elementSize = PageBoxSize;
                elementSize.Height -= element.Margin.Height;
                elementSize.Width -= element.Margin.Width;
                Measurements.Add(element.Measure(elementSize));
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
            return new Document();
        }
    }
}
