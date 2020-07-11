using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class TableElement : Grid
    {
        public string? Preamble { get; }
        public int Level { get; }
        public Paragraph Content { get; }
        public PageReference Reference { get; }

        private readonly Paragraph original;
        private readonly Paragraph referenceParagraph;
        private readonly Paragraph? preambleParagraph;

        public TableElement(string? preamble, int level, Paragraph content, Paragraph reference)
        {
            Preamble = preamble;
            Columns.Add(new GridLength(GridUnit.Point, level * 10));
            Columns.Add(new GridLength(GridUnit.Star, 1));
            Columns.Add(new GridLength(GridUnit.Auto, 0));

            if (Preamble != null)
            {
                preambleParagraph = Paragraph.FromText(Preamble);
                preambleParagraph.Parent = this;
                Set(preambleParagraph, 0, 0);
            }
            Level = level;
            original = reference;
            Content = content.Clone();
            Content.Margin = new Thickness(3, 0);
            Content.FontSize = 11;
            Reference = new PageReference(original);
            referenceParagraph = Paragraph.FromLeaves(Reference);
            referenceParagraph.Parent = this;
            referenceParagraph.HorizontalAlignment = HorizontalAlignment.Right;

            
            Set(Content, 0, 1);
            Set(referenceParagraph, 0, 2);
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            base.OnRender(drawingContext, measurement);

            // render additional dots/lines/whatever
            const double size = 10;
            var lastContentLine = Content.Measurement.Subs[^1];
            var position = Content.Measurement.Position + lastContentLine.Position + lastContentLine.Size;
            var referencePagePosition = referenceParagraph.Measurement.Position;
            var delta = referencePagePosition.X - position.X;
            var count = (int)(delta / size);

            for (int i = 0; i < count; i++)
            {
                referencePagePosition.X -= size;
                drawingContext.Offset = referencePagePosition;
                drawingContext.DrawText(new TextRun(".", new Text.Typeface(Font!, FontSize, FontWeight, FontStyle)), Foreground);
            }
        }

        protected override AbstractElement CloneInternal()
        {
            return new TableElement(Preamble, Level, Content.Clone(), original);
        }
    }
}
