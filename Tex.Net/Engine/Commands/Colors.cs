using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Colors
    {
        [Command("red")]
        public static IEnumerable<Leaf> Red(IEnumerable<Leaf> leaves)
        {
            foreach (var leaf in leaves)
            {
                leaf.Foreground = Color.Red;
            }
            return leaves;
        }
    }
}
