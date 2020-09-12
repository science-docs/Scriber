using Scriber.Drawing;

namespace Scriber.Layout.Document
{
    public class TableElement : Grid
    {
        public string? Preamble { get; }
        public int Level { get; }
        public Paragraph Content { get; }
        public PageReference Reference { get; }

        private readonly AbstractElement original;
        private readonly Paragraph referenceParagraph;
        private readonly Paragraph? preambleParagraph;

        public TableElement(string? preamble, int level, Paragraph content, AbstractElement reference)
        {
            Preamble = preamble;
            Margin = new Thickness(3, 0);
            Columns.Add(new GridLength(GridUnit.Point, CalculateLeft(level)));
            Columns.Add(new GridLength(GridUnit.Star, 1));
            Columns.Add(new GridLength(GridUnit.Auto, 0));

            if (Preamble != null)
            {
                preambleParagraph = Paragraph.FromText(Preamble);
                preambleParagraph.Parent = this;
                preambleParagraph.Margin = new Thickness(0, CalculateLeft(level - 1), 0, 0);
                Set(preambleParagraph, 0, 0);
            }
            Level = level;
            original = reference;
            Content = content.Clone();
            Content.Margin = new Thickness(0);
            Content.FontSize = 11;
            Reference = new PageReference(original);
            referenceParagraph = Paragraph.FromLeaves(Reference);
            referenceParagraph.Parent = this;
            referenceParagraph.VerticalAlignment = VerticalAlignment.Bottom;

            
            Set(Content, 0, 1);
            Set(referenceParagraph, 0, 2);
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            base.OnRender(drawingContext, measurement);

            // render additional dots/lines/whatever
            const double size = 9;
            var lastContentLine = Content.Measurement.Subs[^1];
            var position = Content.Measurement.Position + lastContentLine.Position;
            position.X += lastContentLine.Size.Width + FontSize / 3;
            var referencePagePosition = referenceParagraph.Measurement.Position;
            referencePagePosition.X -= FontSize / 5;
            referencePagePosition.X -= referencePagePosition.X % size;
            var delta = referencePagePosition.X - position.X;
            var count = (int)(delta / size);
            referencePagePosition.Y = position.Y;

            var typeface = new Text.Typeface(Font!, FontSize, FontWeight, FontStyle);
            for (int i = 0; i < count; i++)
            {
                referencePagePosition.X -= size;
                drawingContext.Offset = referencePagePosition;
                drawingContext.DrawText(new TextRun(".", typeface), Foreground);
            }
        }

        private static double CalculateLeft(int level)
        {
            double value = 0;

            for (int i = 0; i < level; i++)
            {
                value += (i + 2) * 8.5;
            }

            return value;
        }

        protected override AbstractElement CloneInternal()
        {
            return new TableElement(Preamble, Level, Content.Clone(), original);
        }
    }
}
