using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Sections
    {
        [Command("section")]
        public static Paragraph Section(CompilerState state, Paragraph content)
        {
            var pretext = "1 ";
            var text = new Layout.Document.Text
            {
                Content = pretext,
                Font = state.Environments.Current.Font,
                FontSize = state.Environments.Current.FontSize.Point
            };
            content.Leaves.Insert(0, text);
            content.FontSize = 16;

            return content;
        }
    }
}
