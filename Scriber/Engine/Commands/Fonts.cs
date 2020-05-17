using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Fonts
    {
        [Command("fontfamily")]
        public static Paragraph Fontfamily(CompilerState state, string fontName, Paragraph paragraph)
        {
            return paragraph;
        }
    }
}
