using Scriber.Layout;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Lists
    {
        [Command("ListItem")]
        public static Paragraph Item(Paragraph content)
        {
            return new ListItem(content);
        }

        [Command("List")]
        public static StackPanel List(ListStyle style, Argument[] contents)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Tag = "list"
            };

            int index = 1;
            foreach (var content in contents)
            {
                if (content.Value is ListItem item)
                {
                    item.Index = index++;
                    item.ListStyle = style;
                    panel.Elements.Add(item);
                }
            }

            return panel;
        }
    }
}
