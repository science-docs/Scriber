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
    public class Document : DocumentElement
    {
        public DocumentVariable Variables { get; } = new DocumentVariable();

        public List<DocumentPage> Pages { get; } = new List<DocumentPage>();

        public PageNumberingModule PageNumbering { get; } = new PageNumberingModule();

        public DocumentElementCollection<Block> Elements { get; }

        public DocumentElementCollection<Block> PageItems { get; }

        public Measurements Measurements { get; }

        public Document()
        {
            Elements = new DocumentElementCollection<Block>(this);
            PageItems = new DocumentElementCollection<Block>(this);
            Measurements = new Measurements();
            
            Font = Text.Font.Serif;
            FontSize = 11;

            DocumentVariables.Setup(Variables);
        }

        public override void Interlude()
        {
            foreach (var element in Elements)
            {
                element.Interlude();
            }

            Measurements.Clear();
            Pages.Clear();
            PageNumbering.Set(1);
        }

        public void Measure()
        {
            var pageSize = Variables["page"]["size"].GetValue<Size>();
            var margin = Variables["page"]["margin"].GetValue<Thickness>();

            var pageBox = pageSize;
            pageBox.Height -= margin.Height;
            pageBox.Width -= margin.Width;

            foreach (var element in Elements)
            {
                element.Document = this;
                var elementSize = pageBox;
                elementSize.Height -= element.Margin.Height;
                elementSize.Width -= element.Margin.Width;
                Measurements.AddInternal(element.Measure(elementSize));
            }

            foreach (var element in PageItems)
            {
                element.Document = this;
            }
        }

        public void Reflow()
        {
            var pageSize = Variables["page"]["size"].GetValue<Size>();
            var margin = Variables["page"]["margin"].GetValue<Thickness>();

            var pageBox = pageSize;
            pageBox.Height -= margin.Height;
            pageBox.Width -= margin.Width;

            foreach (var element in GetAllInternalElements())
            {
                element.Document = this;
                var elementSize = pageBox;
                elementSize.Height -= element.Margin.Height;
                elementSize.Width -= element.Margin.Width;
                element.Measure(elementSize);
            }
        }

        public void Arrange()
        {
            var pageSize = Variables["page"]["size"].GetValue<Size>();
            var margin = Variables["page"]["margin"].GetValue<Thickness>();
            var boxSize = pageSize;
            boxSize.Height -= margin.Height;
            boxSize.Width -= margin.Width;

            var startOffset = new Position(margin.Left, margin.Top);
            var currentOffset = startOffset;
            var contentArea = new Rectangle(startOffset, boxSize);

            var page = AddPage(pageSize, contentArea, PageNumbering.Next());

            foreach (var measurement in Measurements)
            {
                var size = measurement.Size;

                if (currentOffset.Y - startOffset.Y + size.Height > boxSize.Height)
                {
                    currentOffset = startOffset;
                    page = AddPage(pageSize, contentArea, PageNumbering.Next());
                }

                if (measurement.Element.Page == null)
                {
                    measurement.Element.Page = page;
                }

                if (measurement.Element.IsVisible)
                {
                    currentOffset.Y += measurement.Margin.Top;
                }

                var visualOffset = new Position(currentOffset.X, currentOffset.Y);
                visualOffset.X += measurement.Margin.Left;

                measurement.Position = visualOffset;
                Realign(measurement, boxSize, measurement.Element.Alignment);

                if (measurement.Element.IsVisible)
                {
                    currentOffset.Y += size.Height;
                    currentOffset.Y += measurement.Margin.Bottom;
                }
                page.Measurements.Add(measurement);
            }
        }

        public DocumentPage AddPage(Size size, Rectangle contentArea, string pageNumber)
        {
            var page = new DocumentPage(this)
            {
                Size = size,
                ContentArea = contentArea,
                Number = pageNumber
            };

            foreach (var pageItem in PageItems)
            {
                var clone = pageItem.Clone();
                clone.Parent = this;
                clone.Page = page;
                clone.Interlude();
                var measures = clone.Measure(size);
                page.Measurements.AddInternal(measures);
            }

            Pages.Add(page);
            return page;
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

        protected override DocumentElement CloneInternal()
        {
            return new Document();
        }

        private IEnumerable<Block> GetAllInternalElements()
        {
            HashSet<Block> elements = new HashSet<Block>();

            foreach (var page in Pages)
            {
                foreach (var measurement in page.Measurements)
                {
                    elements.Add(measurement.Element);
                }
            }

            return elements;
        }

        private void Realign(Measurement measurement, Size space, Alignment alignment)
        {
            var size = measurement.Size;
            Position pos = measurement.Position;

            switch (alignment)
            {
                case Alignment.Left:
                    pos.X = space.Width - size.Width;
                    break;
                case Alignment.Center:
                    pos.X = (space.Width - size.Width) / 2;
                    break;
            }

            measurement.Position = pos;
        }
    }
}
