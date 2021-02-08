using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Layout.Styling;
using Scriber.Localization;
using Scriber.Variables;
using System;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Tables
    {
        [Command("Table")]
        public static StackPanel Table(CompilerState state, Argument<DocumentElement>[] content)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Flexible = true,
                Tag = "figure"
            };

            var elements = content.Select(e => e.Value).OfType<DocumentElement>().ToArray();

            panel.Elements.AddRange(elements);

            var caption = elements.OfType<Paragraph>().Where(e => "caption".Equals(e.Tag, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (caption != null)
            {
                var copiedCaption = caption.Clone();
                copiedCaption.Tag = "p";

                var tableCount = TableVariables.TableCounter.Increment(state.Document);

                var text = new TextLeaf
                {
                    Content = $"{state.Document.Locale.GetTerm(Term.Table)} {tableCount}: "
                };
                caption.Leaves.Insert(0, text);

                var tables = TableVariables.TableOfTables.Get(state.Document) ?? throw new InvalidOperationException();
                var tableElement = new TableElement(tableCount.ToString(), 1, copiedCaption, panel);
                tables.Add(tableElement);
            }

            return panel;
        }

        [Command("TableOfTables")]
        public static CallbackBlock TableOfTables(CompilerState state)
        {
            return new CallbackBlock(() =>
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                var tables = TableVariables.TableOfTables.Get(state.Document) ?? throw new InvalidOperationException();
                panel.Elements.AddRange(tables);

                return panel;
            });
        }

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
