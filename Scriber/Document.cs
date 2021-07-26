using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Logging;
using Scriber.Bibliography;
using Scriber.Localization;
using Scriber.Variables;
using System.Threading.Tasks;
using System.Linq;
using Scriber.Text;
using Scriber.Layout.Styling;
using System.Diagnostics.CodeAnalysis;
using Scriber.Drawing.Pdf;

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

        public Locale Locale => new(Culture);

        public Citations Citations { get; } = new Citations();

        public StyleCollection Styles { get; }

        public string? Title { get; set; }
        public string? Author { get; set; }

        public override IEnumerable<Symbol> Symbols => Elements.SelectMany(e => e.Symbols);

        public Document()
        {
            Elements = new ElementCollection<DocumentElement>(this);
            PageItems = new ElementCollection<DocumentElement>(this);
            Styles = new StyleCollection();
            Measurements = new Measurements();
            Document = this;

            foreach (var style in StyleReader.GetDefaultStyles())
            {
                Styles.Add(style);
            }
        }

        [return: MaybeNull]
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
            foreach (var element in Elements)
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

            // In order to add the measurements in the correct order
            // we save them temporarily in an array
            Measurement[] measurements = new Measurement[Elements.Count];

            Parallel.ForEach(Elements, (e, _, i) =>
            {
                e.Document = this;
                measurements[i] = e.Measure(boxSize);
            });

            Measurements.AddRange(measurements);

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
