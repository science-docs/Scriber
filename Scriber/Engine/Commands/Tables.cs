using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Layout.Styling;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Tables
    {
        [Command("Tabular")]
        public static Table BuildTable(TableBuilder tableBuilder)
        {
            var table = new Table();

            if (tableBuilder.Header != null)
            {
                var th = new TableHeader();
                th.AddRange(tableBuilder.Header);
                table.Rows.Add(th);
            }

            if (tableBuilder.Data != null)
            {
                foreach (var row in tableBuilder.Data)
                {
                    var tr = new TableRow();
                    tr.AddRange(row);
                    table.Rows.Add(tr);
                }
            }

            return table;
        }

        [Command("RowSpan")]
        public static AbstractElement RowSpan(AbstractElement element, int span)
        {
            element.Style.Set(StyleKeys.RowSpan, span);
            return element;
        }

        [Command("ColumnSpan")]
        public static AbstractElement ColumnSpan(AbstractElement element, int span)
        {
            element.Style.Set(StyleKeys.ColumnSpan, span);
            return element;
        }

    }

    public class TableBuilder
    {
        public DocumentElement[]? Header { get; set; }
        public DocumentElement[][]? Data { get; set; }
    }
}
