using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Lists
    {
        [Command("item", RequiredEnvironment = "itemize")]
        public static Paragraph Item(CompilerState state, Paragraph content)
        {
            return content;
        }
    }
}
