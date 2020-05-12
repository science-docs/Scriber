using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tex.Net.Drawing;
using Tex.Net.Layout;
using Tex.Net.Layout.Document;

namespace Tex.Net
{
    public class Document : AbstractElement
    {
        public DocumentVariable Variables { get; } = new DocumentVariable();

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

        public Document()
        {
            Elements = new ElementCollection<DocumentElement>(this);
            PageItems = new ElementCollection<DocumentElement>(this);
            Measurements = new Measurements();
            
            Font = Text.Font.Serif;
            FontSize = 11;

            Hyphenator = new Text.Hyphenator("en-us");

            DocumentVariables.Setup(Variables);
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
            PageSize = Variables["page"]["size"].GetValue<Size>();
            PageMargin = Variables["page"]["margin"].GetValue<Thickness>();

            PageBoxSize = new Size(PageSize.Width - PageMargin.Width, PageSize.Height - PageMargin.Height);

            foreach (var element in Elements.AsParallel())
            {
                element.Document = this;
                var elementSize = PageBoxSize;
                elementSize.Height -= element.Margin.Height;
                elementSize.Width -= element.Margin.Width;
                Measurements.AddInternal(element.Measure(elementSize));
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

        protected override AbstractElement CloneInternal()
        {
            return new Document();
        }
    }
}
