using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
