using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Lists
    {
        [Command("ListItem")]
        public static Paragraph Item(CompilerState state, Paragraph content)
        {
            var listItem = new ListItem(content);
            var margin = new Thickness(0, 0, state.Document.Variable(ParagraphVariables.Skip), 0);
            listItem.Margin = margin;
            return listItem;
        }

        [Command("List")]
        public static StackPanel List(CompilerState state, ListStyle style, Argument[] contents)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            int index = 1;
            foreach (var content in contents)
            {
                if (content.Value is ListItem item)
                {
                    item.Index = index++;
                    item.Style = style;
                    panel.Elements.Add(item);
                }
            }

            return panel;
        }
    }
}
