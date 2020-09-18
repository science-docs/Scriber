using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;
using Scriber.Autocomplete;
using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Util;
using System;
using System.IO;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Includes
    {
        [Command("IncludePdf")]
        public static PdfElement? IncludePdf(CompilerState state, [Argument(ProposalProvider = typeof(IncludePdfFileProposalProvider))] Argument<string> path, Argument<PdfIncludeOptions>? options = null)
        {
            var uri = state.FileSystem.Path.ConvertToUri(path.Value);
            var bytes = state.FileSystem.File.ReadAllBytes(uri);

            var fields = options?.Value?.Fields;

            if (fields != null)
            {
                bytes = FillPdfDocument(bytes, fields);
            }

            var document = PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Import);
            var range = options?.Value?.Range;
            if (range != null)
            {
                (int start, int end) = range.Value.GetOffsetAndLength(document.PageCount);
                if (start < 0 || start >= document.PageCount || end < 0 || end >= document.PageCount)
                {
                    throw new CompilerException(options?.Source, $"Invalid range {start}-{end} specified. For the specified document the range is limited to {start}-{end}.");
                }
            }

            var element = new PdfElement(document);
            if (range != null)
            {
                element.Range = range.Value;
            }
            return element;
        }

        private static byte[] FillPdfDocument(byte[] bytes, string[] fields)
        {
            var document = PdfReader.Open(new MemoryStream(bytes), PdfDocumentOpenMode.Modify);

            var acro = document.AcroForm;
            if (acro != null)
            {
                //var graphics = XGraphics.FromPdfForm(acro);
                for (int i = 0; i < fields.Length; i++)
                {
                    var field = acro.Fields[i];
                    var page = GetPageFromField(document, field);
                    var rect = GetLayout(acro.Fields[i], page);
                    var graphics = XGraphics.FromPdfPage(page);
                    var (font, brush) = GetFontBrush(field);
                    Render(graphics, font, brush, rect, fields[i]);
                    graphics.Dispose();
                }
                Flatten(acro);
            }

            var ms = new MemoryStream();
            document.Save(ms, false);
            document.Save("tfl2.pdf");
            document.Close();
            return ms.ToArray();
        }

        private static void Flatten(PdfAcroForm acro)
        {
            for (int idx = 0; idx < acro.Fields.Count(); idx++)
            {
                acro.Fields[idx].Elements.Clear();
            }
        }

        private static XRect GetLayout(PdfAcroField field, PdfPage page)
        {
            var height = page.Height.Point;
            var rect = field.Elements["/Rect"];
            if (rect is PdfArray array)
            {
                var elements = array.ToArray();
                var x = GetValue(elements[0]);
                var y = GetValue(elements[3]);
                var w = GetValue(elements[2]) - x;
                var h = y - GetValue(elements[1]);
                return new XRect(x, height - y, w, h);
            }
            return new XRect();
        }

        private static double GetValue(PdfItem item)
        {
            if (item is PdfReal real)
            {
                return real.Value;
            }
            else if (item is PdfInteger integer)
            {
                return integer.Value;
            }
            return 0;
        }

        private static PdfPage? GetPageFromField(PdfDocument document, PdfAcroField field)
        {
            var focusPageReference = (PdfReference)field.Elements["/P"];
            return document.Pages.Cast<PdfPage>().FirstOrDefault(e => e.Reference == focusPageReference);
        }

        private static void Render(XGraphics graphics, XFont font, XBrush brush, XRect rectangle, string text)
        {
            var options = new XStringFormat()
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };
            graphics.DrawString(text, font, brush, rectangle, options);
        }

        private static (XFont? font, XBrush? brush) GetFontBrush(PdfAcroField field)
        {
            if (field is PdfTextField textField)
            {
                return (textField.Font, new XSolidBrush(textField.ForeColor));
            }
            return (null, null);
        }

        [Command("Include")]
        public static void Include(CompilerState state, [Argument(ProposalProvider = typeof(IncludeFileProposalProvider))] Argument<string> path)
        {
            var uri = state.FileSystem.Path.ConvertToUri(path.Value);
            var text = state.FileSystem.File.ReadAllText(uri);

            var tokens = Language.Lexer.Tokenize(text);
            var elements = Parser.Parse(tokens, state.Context.Logger);
            Compiler.Compile(state, elements.Elements);
        }

        public class PdfIncludeOptions
        {
            public Range? Range { get; set; }
            public string[]? Fields { get; set; }
        }
    }
}
