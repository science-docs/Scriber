using Scriber.Drawing;
using Scriber.Layout.Document;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Layout
{
    public class Table : DocumentElement
    {
        public ElementCollection<TableRow> Rows { get; }

        public int Height => Rows.Count;

        public int Width => Rows.Count == 0 ? 0 : Rows.Max(e => e.Width);

        public Table()
        {
            Tag = "table";
            Rows = new ElementCollection<TableRow>(this);
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            return TableMeasurer.Measure(this, availableSize);
        }

        protected override AbstractElement CloneInternal()
        {
            var table = new Table();

            foreach (var row in Rows)
            {
                table.Rows.Add(row.Clone());
            }
            return table;
        }

        public override IEnumerable<AbstractElement> ChildElements()
        {
            return Rows;
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            foreach (var sub in measurement.Subs)
            {
                sub.Element.Render(drawingContext, sub);
            }
        }

        protected override void ArrangeOverride(Measurement finalMeasurement)
        {
            foreach (var sub in finalMeasurement.Subs)
            {
                sub.Element.Arrange(sub);
            }
        }

        public override void Interlude()
        {
            foreach (var row in Rows)
            {
                row.Interlude();
            }
        }
    }
}
