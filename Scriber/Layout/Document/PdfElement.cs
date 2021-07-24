using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using Scriber.Drawing;
using Scriber.Drawing.Pdf;
using System;

namespace Scriber.Layout.Document
{
    public class PdfElement : DocumentElement
    {
        public Range Range { get; set; }

        public string[] FormInput { get; set; } = Array.Empty<string>();

        //public override bool IsVisible => false;

        private readonly PdfDocument document;

        public PdfElement(PdfDocument document)
        {
            Range = 0..^1;
            this.document = document;
            FormInput = new string[] {
                "4711",
                "Thema",
                "A",
                "B"
            };
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return new Measurement(this, new Size(0, double.PositiveInfinity), new Thickness(0));
        }

        protected internal override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            Render(drawingContext);
        }

        public void Render(IDrawingContext drawingContext)
        {
            if (drawingContext is PdfDrawingContext pdf && pdf.Document != null)
            {
                //if (document.AcroForm != null)
                //{
                //    var acro = document.AcroForm.Fields;
                //    for (int i = 0; i < acro.Elements.Count; i++)
                //    {
                //        var field = acro[i];
                //        FillField(field, i, FormInput[i]);
                //    }
                //}
                

                (int start, int end) = Range.GetOffsetAndLength(document.PageCount);
                for (int i = start; i <= end; i++)
                {
                    var pdfPage = document.Pages[i];
                    pdf.Document.AddPage(pdfPage);
                }
            }
        }

        private void FillField(PdfAcroField field, int index, string value)
        {
            field.Value = new PdfString(value);
            //if (field is PdfTextField textField)
            //{
                
            //}
        }

        protected override AbstractElement CloneInternal()
        {
            return new PdfElement(document)
            {
                Range = Range
            };
        }
    }
}
