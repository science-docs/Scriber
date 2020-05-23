using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Lists
    {
        [Command("Item", RequiredEnvironment = "itemize")]
        public static Paragraph Item(CompilerState state, Paragraph content)
        {
            return content;
        }
    }
}
