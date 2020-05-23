using System;
using System.Collections.Generic;
using System.Text;
using Scriber.Layout.Document;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Pagination
    {
        [Command("Pagebreak")]
        public static Pagebreak Pagebreak()
        {
            return new Pagebreak();
        }

        [Command("ThePage")]
        public static PageReference Thepage()
        {
            // Create self referencing element
            return new PageReference();
        }
    }
}
