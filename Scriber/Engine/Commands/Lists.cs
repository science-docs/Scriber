﻿using Scriber.Layout;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Lists
    {
        [Command("ListItem")]
        public static Paragraph Item(CompilerState state, Paragraph content)
        {
            return new ListItem(content);
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
